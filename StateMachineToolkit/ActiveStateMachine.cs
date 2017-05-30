using System;
using System.Collections.Generic;
using System.Threading;
using Sanford.Threading;

namespace Sanford.StateMachineToolkit
{
    public abstract class ActiveStateMachine : StateMachine, IDisposable
    {
        // Used for queuing events.
        private DelegateQueue queue = new DelegateQueue();

        private SynchronizationContext context;

        private bool disposed = false;

        public ActiveStateMachine()
        {
            context = SynchronizationContext.Current;
        }

        ~ActiveStateMachine()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                queue.Dispose();

                GC.SuppressFinalize(this);

                disposed = true;
            }
        }

        protected override void Initialize(State initialState)
        {
            queue.Send(delegate(object dummy)
            {
                InitializeStateMachine(initialState);
            }, null);
        }

        /// <summary>
        /// Sends an event to the StateMachine.
        /// </summary>
        /// <param name="eventID">
        /// The event ID.
        /// </param>
        /// <param name="args">
        /// The data accompanying the event.
        /// </param>
        public override void Send(int eventID, params object[] args)
        {
            #region Require

            if(!IsInitialized)
            {
                throw new InvalidOperationException();
            }
            else if(IsDisposed)
            {
                throw new ObjectDisposedException("ActiveStateMachine");
            }

            #endregion

            queue.Post(delegate(object state)
            {
                Dispatch(eventID, args);
            }, null);       
        }

        public void SendSynchronously(int eventID, params object[] args)
        {
            #region Require

            if(!IsInitialized)
            {
                throw new InvalidOperationException();
            }
            else if(IsDisposed)
            {
                throw new ObjectDisposedException("ActiveStateMachine");
            }

            #endregion

            queue.Send(delegate(object state)
            {
                Dispatch(eventID, args);
            }, null);       
        }

        protected override void SendPriority(int eventID, params object[] args)
        {
            #region Require

            if(!IsInitialized)
            {
                throw new InvalidOperationException();
            }
            else if(IsDisposed)
            {
                throw new ObjectDisposedException("ActiveStateMachine");
            }

            #endregion

            queue.PostPriority(delegate(object state)
            {
                Dispatch(eventID, args);
            }, null);
        }

        /// <summary>
        /// Dispatches events to the current state.
        /// </summary>
        /// <param name="eventID">
        /// The event ID.
        /// </param>
        /// <param name="args">
        /// The data accompanying the event.
        /// </param>
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

                if(context != null)
                {
                    context.Post(delegate(object state)
                    {
                        OnTransitionCompleted(e);
                    }, null);
                }
                else
                {
                    OnTransitionCompleted(e);
                }
            }
        }

        public override StateMachineType StateMachineType
        {
            get 
            {
                return StateMachineType.Active;
            }
        }

        protected bool IsDisposed
        {
            get
            {
                return disposed;
            }
        }

        #region IDisposable Members

        public virtual void Dispose()
        {
            #region Guard

            if(IsDisposed)
            {
                return;
            }

            #endregion

            Dispose(true);
        }

        #endregion
    }
}
