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
using System.Linq;
using Capiche.Common;

namespace Capiche.ApplicationServices
{
    public class ViewManager
    {
        private string[] _viewStates;
        private string DefaultViewState;
        //UI - make this a Dictionary<string, IUserInterface>, if you have to handle more than one
        private IUserInterface _UI;

        public event EventHandler<StateMachineEventArgs> ViewManagerEvent;
        public string CurrentView { get; private set; }
        public IViewStateConfiguration ViewStateConfiguration { get; set; }

        #region singleton implementation
        private static readonly Lazy<ViewManager> _viewManager = new Lazy<ViewManager>(() => new ViewManager());

        public static ViewManager Instance { get { return _viewManager.Value; }}

        private ViewManager()
        {
        }
        #endregion

        public void LoadViewStateConfiguration(IViewStateConfiguration viewStateConfiguration, IUserInterface userInterface)
        {
            ViewStateConfiguration = viewStateConfiguration;
            _viewStates = viewStateConfiguration.ViewStateList;
            _UI = userInterface;
            DefaultViewState = viewStateConfiguration.DefaultViewState;
        }

        public void ViewCommandHandler(object sender, StateMachineEventArgs args)
        {
            try
            {
                if (_viewStates.Contains(args.EventName))
                {
                    _UI.LoadViewState(args.EventName);
                    CurrentView = args.EventName;
                    RaiseViewManagerEvent("View Manager Command", "Successfully loaded view state:" + args.EventName);
                }
                else
                {
                    RaiseViewManagerEvent("View Manager Command", "View state not found!");
                }
            }
            catch (Exception exc)
            {
                RaiseViewManagerEvent("View Manager Command - Error", exc.ToString());
            }
        }

        /// <summary>
        /// handler method for special system events, e.g. initialization
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void SystemEventHandler(object sender, StateMachineEventArgs args)
        {
            //init:
            if (args.EventName == "OnInit")
            {
                _UI.LoadViewState(DefaultViewState);
                CurrentView = DefaultViewState;
            }

            if(args.EventName == "CompleteFailure")
                _UI.LoadViewState("CompleteFailure");
        }

        public void RaiseViewManagerEvent(string eventName, string eventInfo, StateMachineEventType eventType = StateMachineEventType.System)
        {
            var newVMArgs = new StateMachineEventArgs(eventName, "View amanager event: " + eventInfo, eventType, "View Manager");
            if (ViewManagerEvent != null) ViewManagerEvent(this, newVMArgs);
        }

        public void RaiseUICommand(string command, string info, string source, string target)
        {
            var newUIArgs = new StateMachineEventArgs(command, info, StateMachineEventType.Command, source, target);
            if (ViewManagerEvent != null) ViewManagerEvent(this, newUIArgs);
        }
    }
}
