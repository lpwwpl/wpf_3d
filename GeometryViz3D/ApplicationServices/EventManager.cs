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
using Capiche.Common;

namespace Capiche.ApplicationServices
{
    public class EventManager
    {
        //collection of registered events
        private Dictionary<string, object> EventList;

        /// <summary>
        /// event manager event used for logging
        /// </summary>
        public event EventHandler<StateMachineEventArgs> EventManagerEvent;

        #region singleton implementation
        private static readonly Lazy<EventManager> _eventManager = new Lazy<EventManager>(() => new EventManager());

        public static EventManager Instance { get { return _eventManager.Value; }}

        private EventManager()
        {
            EventList = new Dictionary<string, object>();
        }
        #endregion

        /// <summary>
        /// registration of an event used in the system
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="source"></param>
        public void RegisterEvent(string eventName, object source)
        {
            EventList.Add(eventName, source);
        }

        /// <summary>
        /// Subscription method maps handler method in a sink object to an event of the source object.
        /// method signatures between the delegate and handler need to match
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="handlerMethodName"></param>
        /// <param name="sink"></param>
        /// <returns></returns>
        public bool SubscribeEvent(string eventName, string handlerMethodName, object sink)
        {
            try
            {
                //get event from list:
                var evt = EventList[eventName];
                //determine meta data from event and handler:
                var eventInfo = evt.GetType().GetEvent(eventName);
                var methodInfo = sink.GetType().GetMethod(handlerMethodName);
                //create new delegate mapping event to handler:
                Delegate handler = Delegate.CreateDelegate(eventInfo.EventHandlerType, sink, methodInfo);
                eventInfo.AddEventHandler(evt, handler);
                return true;
            }
            catch (Exception exc)
            {
                var message = "Exception thrown while subscribing to handler.  Event:" + eventName + " - Handler: " + handlerMethodName + ":" + exc.ToString();
                RaiseEventManagerEvent("EventManagerSystemEvent", message, StateMachineEventType.System);
                return false;
            }
        }

        private void RaiseEventManagerEvent(string eventName, string eventInfo, StateMachineEventType eventType)
        {
            var newArgs = new StateMachineEventArgs(eventName, eventInfo, eventType, "Event Manager");
            if (EventManagerEvent != null) EventManagerEvent(this, newArgs);
        }
    }
}
