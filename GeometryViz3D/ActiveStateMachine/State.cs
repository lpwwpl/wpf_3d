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
    public class State
    {
        public string StateName { get; private set; }
        public Dictionary<string, Transition> StateTransitionList { get; private set; }
        public List<StateMachineAction> EntryActions { get; private set; }
        public List<StateMachineAction> ExitActions { get; private set; }
        public bool IsDefaultState { get; private set; }

        public State(string stateName, Dictionary<string, Transition> stateTransitionList, List<StateMachineAction> entryActions, List<StateMachineAction> exitActions, bool isDefaultState = false)
        {
            StateName = stateName;
            StateTransitionList = stateTransitionList;
            EntryActions = entryActions;
            ExitActions = exitActions;
            IsDefaultState = isDefaultState;
        }
    }
}
