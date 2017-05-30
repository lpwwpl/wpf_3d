using System;
using System.Collections.Generic;
using Sanford.Collections;

namespace Sanford.StateMachineToolkit
{
    public class PassiveStateMachine : StateMachine
    {
        private Deque eventDeque = new Deque();

        protected override void Initialize(State initialState)
        {
            InitializeStateMachine(initialState);
        }

        public void Execute()
        {
            StateMachineEvent e = null;

            while(eventDeque.Count > 0)
            {
                e = (StateMachineEvent)eventDeque.PopFront();

                Dispatch(e.EventID, e.GetArgs());
            }
        }

        public override void Send(int eventID, params object[] args)
        {
            #region Require

            if(!IsInitialized)
            {
                throw new InvalidOperationException("State machine has not been initialized.");
            }

            #endregion

            eventDeque.PushBack(new StateMachineEvent(eventID, args));
        }

        protected override void SendPriority(int eventID, params object[] args)
        {
            #region Require

            if(!IsInitialized)
            {
                throw new InvalidOperationException("State machine has not been initialized.");
            }

            #endregion
            
            eventDeque.PushFront(new StateMachineEvent(eventID, args));
        }

        private void Dispatch(int eventID, object[] args)
        {
            // Reset action result.
            ActionResult = null;

            TransitionResult result;

            // Dispatch event to the current state.
            result = currentState.Dispatch(eventID, args);

            // If a transition was fired as a result of this event.
            if(result.HasFired)
            {
                currentState = result.NewState;

                TransitionCompletedEventArgs e = new TransitionCompletedEventArgs(currentState.ID, eventID, ActionResult, result.Error);

                OnTransitionCompleted(e);
            }
        }

        public override StateMachineType StateMachineType
        {
            get
            {
                return StateMachineType.Passive;
            }
        }

        private class StateMachineEvent
        {
            private int eventID;

            private object[] args;

            public StateMachineEvent(int eventID, object[] args)
            {
                this.eventID = eventID;
                this.args = args;
            }

            public object[] GetArgs()
            {
                return args;
            }

            public int EventID
            {
                get
                {
                    return eventID;
                }
            }
        }
    }
}
