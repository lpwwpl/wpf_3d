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
using System.Diagnostics;

namespace Sanford.StateMachineToolkit
{
    /// <summary>
    /// Represents the method that will perform an action during a state 
    /// transition.
    /// </summary>
    public delegate void ActionHandler(object[] args);

    /// <summary>
    /// Represents the method that is evaluated to determine whether the state
    /// transition should fire.
    /// </summary>
    public delegate bool GuardHandler(object[] args);

    /// <summary>
    /// Represents the method that is called when a state is entered.
    /// </summary>
    public delegate void EntryHandler();

    /// <summary>
    /// Represents the method that is called when a state is exited.
    /// </summary>
    public delegate void ExitHandler();

    /// <summary>
    /// Specifies constants defining the type of history a state uses.
    /// </summary>
    /// <remarks>
    /// A state's history type determines which of its nested states it enters 
    /// into when it is the target of a transition. If a state does not have 
    /// any nested states, its history type has no effect.
    /// </remarks>
    public enum HistoryType
    {
        /// <summary>
        /// The state enters into its initial state which in turn enters into
        /// its initial state and so on until the innermost nested state is 
        /// reached.
        /// </summary>
        None,

        /// <summary>
        /// The state enters into its last active state which in turn enters 
        /// into its initial state and so on until the innermost nested state
        /// is reached.
        /// </summary>
        Shallow,

        /// <summary>
        /// The state enters into its last active state which in turns enters
        /// into its last active state and so on until the innermost nested
        /// state is reached.
        /// </summary>
        Deep
    }

	/// <summary>
	/// Represents a state of the state machine.
	/// </summary>
	public class State
	{
        #region State Members

        #region Fields

        // The superstate.
        private State superstate = null;

        // The initial State.
        private State initialState = null;

        // The history State.
        private State historyState = null;

        // The collection of substates for the State.
        private SubstateCollection substates;

        // The collection of Transitions for the State.
        private TransitionCollection transitions;

        // The result if no transitions fired in response to an event.
        private TransitionResult notFiredResult = new TransitionResult(false, null, null);        

        // Entry action.
        private EntryHandler entryHandler = null;

        // Exit action.
        private ExitHandler exitHandler = null;        

        // The State's history type.
        private HistoryType historyType = HistoryType.None;  

        // The level of the State within the State hierarchy.
        private int level;  

        // A unique integer value representing the State's ID.
        private int stateID;
      
        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the State class with the specified
        /// number of events it will handle.
        /// </summary>
        /// <param name="stateID">
        /// The State's ID.
        /// </param>
        public State(int stateID)
        {
            InitializeState(stateID);
        }

        /// <summary>
        /// Initializes a new instance of the State class with the specified
        /// number of events it will handle as well as its entry action.
        /// </summary>
        /// <param name="stateID">
        /// The State's ID.
        /// </param>
        /// <param name="entryHandler">
        /// The entry action.
        /// </param>
        public State(int stateID, EntryHandler entryHandler)
        {
            this.entryHandler = entryHandler;

            InitializeState(stateID);
        }

        /// <summary>
        /// Initializes a new instance of the State class with the specified
        /// number of events it will handle as well as its exit action.
        /// </summary>
        /// <param name="stateID">
        /// The State's ID.
        /// </param>
        /// <param name="exitHandler">
        /// The exit action.
        /// </param>
        public State(int stateIDt, ExitHandler exitHandler)
        {
            this.exitHandler = exitHandler;

            InitializeState(stateID);
        }

        /// <summary>
        /// Initializes a new instance of the State class with the specified
        /// number of events it will handle as well as its entry and exit 
        /// actions.
        /// </summary>
        /// <param name="stateID">
        /// The State's ID.
        /// </param>
        /// <param name="entryHandler">
        /// The entry action.
        /// </param>
        /// <param name="exitHandler">
        /// The exit action.
        /// </param>
        public State(int stateID, EntryHandler entryHandler, ExitHandler exitHandler)
        {
            this.entryHandler = entryHandler;
            this.exitHandler = exitHandler;

            InitializeState(stateID);
        }

        #endregion

        #region Methods

        // Initializes the State.
        private void InitializeState(int stateID)
        {
            this.stateID = stateID;

            substates = new SubstateCollection(this);
            transitions = new TransitionCollection(this);

            level = 1;
        }
        
        /// <summary>
        /// Dispatches an event to the StateMachine.
        /// </summary>
        /// <param name="args">
        /// The arguments accompanying the event.
        /// </param>
        /// <returns>
        /// The results of the dispatch.
        /// </returns>
        internal TransitionResult Dispatch(int eventID, object[] args)
        { 
            return Dispatch(this, eventID, args);
        }        
        
        // Recursively goes up the the state hierarchy until a state is found 
        // that will handle the event.
        private TransitionResult Dispatch(State origin, int eventID, object[] args)
        {       
            TransitionResult transResult = notFiredResult;

            // If there are any Transitions for this event.
            if(transitions[eventID] != null)
            {
                // Iterate through the Transitions until one of them fires.
                foreach(Transition trans in transitions[eventID])
                {
                    transResult = trans.Fire(origin, args);

                    if(transResult.HasFired)
                    {                   
                        // Break out of loop. We're finished.
                        break;
                    }
                }
            }
            // Else if there are no Transitions for this event and there is a 
            // superstate.
            else if(Superstate != null)
            {
                // Dispatch the event to the superstate.
                transResult = Superstate.Dispatch(origin, eventID, args);
            }

            return transResult;
        }

        /// <summary>
        /// Enters the state.
        /// </summary>
        internal void Entry()
        {
            // If an entry action exists for this state.
            if(entryHandler != null)
            {
                // Execute entry action.
                entryHandler();
            }
        }

        /// <summary>
        /// Exits the state.
        /// </summary>
        internal void Exit()
        {
            // If an exit action exists for this state.
            if(exitHandler != null)
            {
                // Execute exit action.
                exitHandler();
            }


            // If there is a superstate.
            if(superstate != null)
            {
                // Set the superstate's history state to this state. This lets
                // the superstate remember which of its substates was last 
                // active before exiting.
                superstate.historyState = this;
            }
        }

        // Enters the state by its history (assumes that the Entry method has 
        // already been called).
        internal State EnterByHistory()
        {
            State result = this;

            // If there is no history type.
            if(HistoryType == HistoryType.None)
            {
                // If there is an initial state.
                if(initialState != null)
                {
                    // Enter the initial state.
                    result = initialState.EnterShallow();
                }
            }
            // Else if the history is shallow.
            else if(HistoryType == HistoryType.Shallow)
            {
                // If there is a history state.
                if(historyState != null)
                {
                    // Enter history state in shallow mode.
                    result = historyState.EnterShallow();
                }
            }
            // Else the history is deep.
            else
            {
                // If there is a history state.
                if(historyState != null)
                {
                    // Enter history state in deep mode.
                    result = historyState.EnterDeep();
                }
            }

            return result;
        }

        // Enters the state in via its history in shallow mode.
        private State EnterShallow()
        {
            Entry();

            State result = this;

            // If the lowest level has not been reached.
            if(initialState != null)
            {
                // Enter the next level initial state.
                result = initialState.EnterShallow();
            }

            return result;
        }

        // Enters the state via its history in deep mode.
        private State EnterDeep()
        {
            Entry();

            State result = this;

            // If the lowest level has not been reached.
            if(historyState != null)
            {
                // Enter the next level history state.
                result = historyState.EnterDeep();
            }

            return result;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the State's ID.
        /// </summary>
        public int ID
        {
            get
            {
                return stateID;
            }
        }

        /// <summary>
        /// Gets the collection of substates.
        /// </summary>
        public SubstateCollection Substates
        {
            get
            {
                return substates;
            }
        }

        /// <summary>
        /// Gets the collection of transitions.
        /// </summary>
        public TransitionCollection Transitions
        {
            get
            {
                return transitions;
            }
        }

        /// <summary>
        /// Gets or sets the superstate.
        /// </summary>
        /// <remarks>
        /// If no superstate exists for this state, this property is null.
        /// </remarks>
        internal State Superstate
        {
            get
            {
                return superstate;
            }
            set
            {
                #region Preconditions

                if(this == value)
                {
                    throw new ArgumentException(
                        "The superstate cannot be the same as this state.");
                }

                #endregion

                superstate = value;

                if(superstate == null)
                {
                    Level = 1;
                }
                else
                {
                    Level = superstate.Level + 1;
                }
            }
        }

        /// <summary>
        /// Gets or sets the initial state.
        /// </summary>
        /// <remarks>
        /// If no initial state exists for this state, this property is null.
        /// </remarks>
        public State InitialState
        {
            get
            {
                return initialState;
            }
            set
            {
                #region Preconditions

                if(this == value)
                {
                    throw new ArgumentException(
                        "State cannot be an initial state to itself.");
                }
                else if(value.Superstate != this)
                {
                    throw new ArgumentException(
                        "State is not a direct substate.");
                }

                #endregion

                initialState = historyState = value; 
            }
        }

        /// <summary>
        /// Gets or sets the history type.
        /// </summary>
        public HistoryType HistoryType
        {
            get
            {
                return historyType;
            }
            set
            {
                historyType = value;
            }
        }

        /// <summary>
        /// Gets the State's level in the State hierarchy.
        /// </summary>
        internal int Level
        {
            get
            {
                return level;
            }
            set
            {
                level = value;

                foreach(State substate in Substates)
                {
                    substate.Level = level + 1;
                }
            }
        }

        #endregion

        #endregion
	}
}
