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
using System.Text;
using System.Threading.Tasks;

namespace Capiche.ActiveStateMachine
{
    public class StateMachineAction
    {
        #region public members
        public string Name { get; private set; } 
        #endregion

        #region private members
        //delegate pointing to the implementation of the method to be executed
        private Action _method;
        #endregion

        /// <summary>
        /// ctor for state machine action
        /// </summary>
        /// <param name="name"></param>
        /// <param name="method"></param>
        public StateMachineAction(string name, Action method)
        {
            Name = name;
            _method = method;
        }

        /// <summary>
        /// Method running the action.
        /// Will be called e.g. by the state machine, when a transition is executed.
        /// Could also be used as a guard, entry or exit action
        /// </summary>
        public void Execute()
        {
            //invoke the state machine action method:
            _method.Invoke();
        }
    }
}
