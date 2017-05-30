#region License

/* Copyright (c) 2006 Leslie Sanford
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy 
 * of this software and associated documentation files (the "Software"), to 
 * deal in the Software without restriction, including without limitation the 
 * rights to use, copy, modify, merge, publish, distribute, sublicense, and/or 
 * sell copies of the Software, and to permit persons to whom the Software is 
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in 
 * all copies or substantial portions of the Software. 
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
 * THE SOFTWARE.
 */

#endregion

#region Contact

/*
 * Leslie Sanford
 * Email: jabberdabber@hotmail.com
 */

#endregion

using System;

namespace Sanford.StateMachineToolkit
{
    /// <summary>
    /// Summary description for TransitionCompletedEventArgs.
    /// </summary>
    public class TransitionCompletedEventArgs : EventArgs
    {
        private int stateID;

        private int eventID;

        private object actionResult = null;

        private Exception error = null;

        public TransitionCompletedEventArgs(int stateID, int eventID, object actionResult, Exception error)
        {
            this.stateID = stateID;
            this.eventID = eventID;
            this.actionResult = actionResult;
            this.error = error;
        }

        public int StateID
        {
            get
            {
                return stateID;
            }
        }

        public int EventID
        {
            get
            {
                return eventID;
            }
        }

        public object ActionResult
        {
            get
            {
                return actionResult;
            }
        }

        public Exception Error
        {
            get
            {
                return error;
            }
        }
    }

    public delegate void TransitionCompletedEventHandler(object sender, TransitionCompletedEventArgs e);
}
