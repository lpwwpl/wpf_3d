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

namespace Sanford.StateMachineToolkit
{
	/// <summary>
	/// Represents a transition from one state to another.
	/// </summary>
    public class Transition
    {
        #region Transition Members

        #region Fields

        // The source of the transition.
        private State source = null;

        // The target of the transition.
        private State target = null;

        // The guard to evaluate to determine whether the transition should fire.
        private GuardHandler guard = null;

        // The actions to perform during the transition.
        private ActionCollection actions = new ActionCollection();

        // If an exception is thrown from an action, represents the exception thrown.
        private Exception exceptionResult = null;

        // The result if the transition did not fire.
        private static readonly TransitionResult notFiredResult = new TransitionResult(false, null, null);        

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the Transition class.
        /// </summary>
        public Transition()
        {
        }

        /// <summary>
        /// Initializes a new instance of the Transition class with the 
        /// specified target.
        /// </summary>
        /// <param name="target">
        /// The target state of the transition.
        /// </param>
        public Transition(State target)
        {
            this.target = target;
        }

        /// <summary>
        /// Initializes a new instance of the Transition class with the 
        /// specified guard.
        /// </summary>
        /// <param name="guard">
        /// The guard to test to determine whether the transition should take 
        /// place.
        /// </param>
        public Transition(GuardHandler guard)
        {
            this.guard = guard;
        }

        /// <summary>
        /// Initializes a new instance of the Transition class with the 
        /// specified guard and target.
        /// </summary>
        /// <param name="guard">
        /// The guard to test to determine whether the transition should take 
        /// place.
        /// </param>
        /// <param name="target">
        /// The target state of the transition.
        /// </param>
        public Transition(GuardHandler guard, State target)
        {
            this.guard = guard;
            this.target = target;
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Fires the transition.
        /// </summary>
        /// <param name="origin">
        /// The State that originally received the event.
        /// </param>
        /// <param name="args">
        /// The arguments accompanying the event.
        /// </param>
        /// <returns>
        /// A TransitionResult object representing the results of the transition.
        /// </returns>
        internal TransitionResult Fire(State origin, object[] args)
        {
            TransitionResult result;            

            // If the transition should fire.
            if(ShouldFire(args))
            {
                State newState = origin;

                // If this is not an internal transition.
                if(Target != null)
                {                
                    State o = origin;

                    // Unwind up from the state that originally received the event 
                    // to the source state.
                    while(o != Source)
                    {
                        o.Exit();
                        o = o.Superstate;
                    }

                    Fire(Source, Target, args);

                    newState = Target.EnterByHistory();
                }
                // Else if this is an internal transition.
                else
                {
                    PerformActions(args);
                }

                result = new TransitionResult(true, newState, exceptionResult);
            }
            // Else the transition should not fire.
            else
            {
                result = notFiredResult;
            }

            return result;
        }

        // Recursively traverses the state hierarchy, exiting states along 
        // the way, performing the action, and entering states to the target.
        private void Fire(State s, State t, object[] args)
        {
            /*
             * There are several state transition traversal cases:
             * 
             * 1. The source and target are the same (self-transition).
             * 2. The target is a substate of the source.
             * 3. The source is a substate of the target.
             * 4. The source and target share the same superstate.
             * 5. All other cases.
             *     a. The source and target reside at the save level in the 
             *        hiearchy (but do not share the same superstate).
             *     b. The source is lower in the hiearchy than the target.
             *     c. The target is lower in the hierarchy than the source.
             * 
             * Case 1: Immediately performs the transition.
             * 
             * Case 2: Traverses the hierarchy from the source to the target, 
             *         entering each state along the way. No states are exited.
             * 
             * Case 3: Traverses the hierarchy from the source to the target, 
             *         exiting each state along the way. The target is then 
             *         entered.
             * 
             * Case 4: The source is exited and the target entered.
             * 
             * Case 5: Traverses the hiearchy until a common superstate is met.
             * 
             * The action is performed between the last state exit and first state
             * entry.
             */
            
            // Handles case 1.
            // Handles case 3 after traversing from the source to the target.
            if(s == Target)
            {
                s.Exit();
                PerformActions(args);
                Target.Entry();
            }
            // Handles case 2 after traversing from the target to the source.
            else if(s == t)
            {
                PerformActions(args);
                return;
            }
            // Handles case 4.
            // Handles case 5a after traversing the hierarchy until a common 
            // ancestor if found.
            else if(s.Superstate == t.Superstate)
            {
                s.Exit();
                PerformActions(args);
                t.Entry();
            }
            else
            {
                /*
                 * The following traverses the hierarchy until one of the above
                 * conditions is met.
                 */

                // Handles case 3.
                // Handles case 5b.
                if(s.Level > t.Level)
                {
                    s.Exit();
                    Fire(s.Superstate, t, args);
                }
                // Handles case 2.
                // Handles case 5c.
                else if(s.Level < t.Level)
                {
                    Fire(s, t.Superstate, args);
                    t.Entry();
                }
                // Handles case 5a.
                else
                {
                    s.Exit();
                    Fire(s.Superstate, t.Superstate, args);
                    t.Entry();
                }
            }
        }
      
        // Returns a value indicating whether or not the transition should fire.
        private bool ShouldFire(object[] args)
        {
            bool result = true;

            // If there is a guard and it does not evaluate to true.
            if(Guard != null && !Guard(args))
            {
                // Guard must evaluate to true for transition to fire.
                result = false;
            }

            return result;
        }

        // Performs the transition's actions.
        private void PerformActions(object[] args)
        {
            exceptionResult = null;

            try
            {
                foreach(ActionHandler action in Actions)
                { 
                    action(args);
                }
            }
            catch(Exception ex)
            {
                exceptionResult = ex;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the guard to test to determine if the transition should take 
        /// place.
        /// </summary>
        /// <remarks>
        /// If no guard is necessary, this value may be null.
        /// </remarks>
        public GuardHandler Guard
        {
            get
            {
                return guard;
            }
        }

        /// <summary>
        /// Gets the collection of actions.
        /// </summary>
        public ActionCollection Actions
        {
            get
            {
                return actions;
            }
        }

        /// <summary>
        /// Gets the target of the transition.
        /// </summary>
        public State Target
        {
            get
            {
                return target;
            }
        } 

        /// <summary>
        /// Gets or sets the source of the transition.
        /// </summary>
        internal State Source
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
            }
        }

        #endregion

        #endregion
    }
}
