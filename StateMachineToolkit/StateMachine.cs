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
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace Sanford.StateMachineToolkit
{
    public enum StateMachineType
    {
        Passive,
        Active
    }

	/// <summary>
	/// Represents the base class for all state machines.
	/// </summary>
	public abstract class StateMachine
	{
        #region StateMachine Members

        #region Fields

        // The current state.
        protected State currentState = null;

        // The return value of the last action.
        private object actionResult;

        // Indicates whether the state machine has been initialized.
        private bool initialized = false;        

        #endregion

        #region Events

        public event TransitionCompletedEventHandler TransitionCompleted;

        #endregion 

        #region Methods

        /// <summary>
        /// Initializes the StateMachine's initial state.
        /// </summary>
        /// <param name="initialState">
        /// The state that will initially receive events from the StateMachine.
        /// </param>
        protected abstract void Initialize(State initialState);

        protected void InitializeStateMachine(State initialState)
        {
            #region Require

            if(initialState == null)
            {
                throw new ArgumentNullException("initialState");
            }
            else if(IsInitialized)
            {
                throw new InvalidOperationException("The state machine has already been initialized.");
            }

            #endregion

            initialized = true;

            State superstate = initialState;
            Stack superstateStack = new Stack();

            // If the initial state is a substate, travel up the state 
            // hierarchy in order to descend from the top state to the initial
            // state.
            while(superstate != null)
            {
                superstateStack.Push(superstate);
                superstate = superstate.Superstate;
            }

            // While there are superstates to traverse.
            while(superstateStack.Count > 0)
            {
                superstate = (State)superstateStack.Pop();
                superstate.Entry();
            }

            currentState = initialState.EnterByHistory();
        }

        public abstract void Send(int eventID, params object[] args);

        protected abstract void SendPriority(int eventID, params object[] args);

        protected virtual void OnTransitionCompleted(TransitionCompletedEventArgs e)
        {
            TransitionCompletedEventHandler handler = TransitionCompleted;

            if(handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the ID of the current State.
        /// </summary>
        public int CurrentStateID
        {
            get
            {
                #region Require

                if(!initialized)
                {
                    throw new InvalidOperationException();
                }

                #endregion

                Debug.Assert(currentState != null);

                return currentState.ID;
            }
        }

        /// <summary>
        /// Gets or sets the results of the action performed during the last transition.
        /// </summary>
        /// <remarks>
        /// This property should only be set during the execution of an action method.
        /// </remarks>
        protected object ActionResult
        {
            get
            {
                return actionResult;
            }
            set
            {
                actionResult = value;
            }
        }

        public abstract StateMachineType StateMachineType
        {
            get;
        }

        protected bool IsInitialized
        {
            get
            {
                return initialized;
            }
        }        

        #endregion

        #endregion        
    }
}
