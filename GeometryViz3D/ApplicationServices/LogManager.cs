﻿/*
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
using System.Diagnostics;
using Capiche.Common;

namespace Capiche.ApplicationServices
{
    public class LogManager
    {
        #region singleton implementation
        private static readonly Lazy<LogManager> _logManager = new Lazy<LogManager>(() => new LogManager());

        public static LogManager Instance { get { return _logManager.Value; } }

        private LogManager()
        {
        }
        #endregion

        public void LogEventHandler(object sender, StateMachineEventArgs args)
        {
            if (args.EventType != StateMachineEventType.Notification)
            {
                Debug.Print(args.TimeStamp + " SystemEvent:" + args.EventName + 
                    " - Info: " + args.EventInfo + " - StateMachineArgumentType: " + args.EventType + " - Source: " + args.Source + " - Target: " + args.Target);
            }
            else
            {
                Debug.Print(args.TimeStamp + " Notification:" + args.EventName +
                    " - Info: " + args.EventInfo + " - StateMachineArgumentType: " + args.EventType + " - Source: " + args.Source + " - Target: " + args.Target);
            }
        }
    }
}
