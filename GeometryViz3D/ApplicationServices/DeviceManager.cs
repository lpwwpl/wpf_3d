/*
author: Mark Cafazzo
email: mark.cafazzo@capacitive.ca

Licensees of this software and components thereof are granted free use under the MIT License (MIT):

Copyright © 2015 Mark Cafazzo and Capacitive Technologies Inc.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Capiche.Common;

namespace Capiche.ApplicationServices
{
    /// <summary>
    /// Manages all devices
    /// </summary>
    public class DeviceManager
    {
        
        #region singleton implementation
            // Create a thread-safe singleton with lazy initialization
            private static readonly Lazy<DeviceManager> _deviceManager =new Lazy<DeviceManager>(() => new DeviceManager());

            public static DeviceManager Instance { get { return _deviceManager.Value; } }

            private DeviceManager()
            {
                DeviceList = new Dictionary<string, object>();
            }
        #endregion

        // Public members
        
        /// <summary>
        /// List of system devices
        /// </summary>
        private Dictionary<string, object> DeviceList { get; set; }

        // Public members
        // Device manager event is used for logging
        public event EventHandler<StateMachineEventArgs> DeviceManagerEvent;
        public event EventHandler<StateMachineEventArgs> DeviceManagerNotification;
       
        // Methods
        
        /// <summary>
        /// Add a system device
        /// </summary>
        /// <param name="name"></param>
        /// <param name="device"></param>
        public void AddDevice(string name, object device)
        {
            DeviceList.Add(name, device);
            RaiseDeviceManagerEvent("Added device", name);
        }

        /// <summary>
        /// Removes a system device
        /// </summary>
        /// <param name="name"></param>
        public void RemoveDevice(string name)
        {
            DeviceList.Remove(name);
            RaiseDeviceManagerEvent("Removed device", name);
        }

        /// <summary>
        /// Handler method for state machine commands
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void DeviceCommandHandler(object sender, StateMachineEventArgs args)
        {

            // Listen to command events only
            if (args.EventType != StateMachineEventType.Command) return;

            // Get device and execute command action method
            try
            {
                if (!DeviceList.Keys.Contains(args.Target)) return;
                // Convention device commands and method names must mach!
                var device = DeviceList[args.Target];
                MethodInfo deviceMethod = device.GetType().GetMethod(args.EventName);
                deviceMethod.Invoke(device, new object[] { });
                RaiseDeviceManagerEvent("DeviceCommand", "Successful device command: " + args.Target + " - " + args.EventName);
            }
            catch (Exception exc)
            {
                RaiseDeviceManagerEvent("DeviceCommand - Error", exc.ToString());
            }
        }

        /// <summary>
        ///  Handler method for special system events, e.g. initialization
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void SystemEventHandler(object sender, StateMachineEventArgs args)
        {
            // Initialize
            if (args.EventName == "OnInit" && args.EventType==StateMachineEventType.Command)
            {
                foreach (var device in DeviceList)
                {
                    try
                    {
                        MethodInfo initMethod = device.Value.GetType().GetMethod("OnInit");
                        initMethod.Invoke(device.Value, new object[] { });
                        RaiseDeviceManagerEvent("DeviceCommand - Initialization of device", device.Key);
                    }
                    catch (Exception exc)
                    {
                        RaiseDeviceManagerEvent("DeviceCommand - Initialization error device" + device.Key, exc.ToString());
                    }
                }
            }

            // Notification handling
            // because we use UI to trigger transitions devices would trigger normally themselves.
            // Nevertheless, this is common, if SW user interfaces control devices
            // View and device managers communicate on system event bus and use notifications to trigger state machine as needed!
            if (args.EventType == StateMachineEventType.Command)
            {
                // Check for right condition 
                if (args.EventName == "OnInit") return;
                if (!DeviceList.ContainsKey(args.Target)) return;
                
                // Dispatch command to device
                DeviceCommandHandler(this, args);
                //RaiseDeviceManagerNotification(args.EventName, "Routed through device manager: " + args.EventInfo, args.Source);
            }


        }

        /// <summary>
        /// Method to raise a device manager event for logging, etc.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="info"></param>
        private void RaiseDeviceManagerEvent(string name, string info)
        {
            var newDMArgs = new StateMachineEventArgs(name, "Device manager event: " + info,
                StateMachineEventType.System, "Device Manager");
            // Raise only, if subscribed
            if (DeviceManagerEvent != null)
            {
                DeviceManagerEvent(this, newDMArgs);
            }
        }


        /// <summary>
        /// Sends a command from device manager to state machine
        /// </summary>
        /// <param name="command"></param>
        /// <param name="info"></param>
        /// <param name="source"></param>
        public void RaiseDeviceManagerNotification(string command, string info, string source)
        {
            var newDMArgs = new StateMachineEventArgs(command, info, StateMachineEventType.Notification, source, "State Machine");
            // Raise only, if subscribed
            if (DeviceManagerNotification != null)
            {
                DeviceManagerNotification(this, newDMArgs);
            }
            
        }

        /// <summary>
        /// Loads device configuration
        /// </summary>
        /// <param name="devManConfiguration"></param>
        public void LoadDeviceConfiguration(IDeviceConfiguration devManConfiguration)
        {
            DeviceList = devManConfiguration.Devices;
        }
    }
}
