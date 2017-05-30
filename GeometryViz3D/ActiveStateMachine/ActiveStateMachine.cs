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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Capiche.Common;

namespace Capiche.ActiveStateMachine
{
    /// <summary>
    /// Base class for active state machines
    /// </summary>
    public class ActiveStateMachine
    {
        #region public members
        public Dictionary<String, State> StateList { get; private set; }
        public BlockingCollection<string> TriggerQueue { get; private set; }
        public State CurrentState { get; private set; }
        public State PreviousState { get; private set; }
        public EngineState StateMachineEngine { get; private set; }
        public event EventHandler<StateMachineEventArgs> StateMachineEvent; 
        #endregion

        // Private members
        private Task _queueWorkerTask;
        private readonly State _initialState;
        private ManualResetEvent _resumer;
        private CancellationTokenSource _tokenSource;

        /// <summary>
        /// Constructor Active State Machine
        /// </summary>
        /// <param name="stateList"></param>
        /// <param name="queueCapacity"></param>
        public ActiveStateMachine(Dictionary<String, State> stateList, int queueCapacity)
        {
            //cnfigure state machine
            StateList = stateList;
            //set initial state:
            _initialState = new State("InitialState", null, null, null);
            //collection taking in all triggers. Is thread-safe, blocking as well as FIFO
            //limiting its capacity protects against DOS-style errors or attacks
            TriggerQueue = new BlockingCollection<string>(queueCapacity);

            //Initialize
            InitStateMachine();
            //raise an event
            RaiseStateMachineSystemEvent("StateMachine: Initialized", "System ready to start");
            StateMachineEngine = EngineState.Initialized;
        }

        #region state machine engine

        /// <summary>
        /// Start the state machine
        /// </summary>
        public void Start()
        {
            //create cancellation token for QueueWorker method
            _tokenSource = new CancellationTokenSource();

            //create a new worker thread, if it does not exist
            _queueWorkerTask = Task.Factory.StartNew(QueueWorkerMethod,_tokenSource, TaskCreationOptions.LongRunning);

            //set engine state:
            StateMachineEngine = EngineState.Running;
            RaiseStateMachineSystemEvent("StateMachine: Started", "System running.");
        }

        /// <summary>
        /// Pauses the state machine worker thread.
        /// </summary>
        public void Pause()
        {
            //set engine state:
            StateMachineEngine = EngineState.Paused;
            _resumer.Reset();
            RaiseStateMachineSystemEvent("StateMachine: Paused", "System waiting.");
        }

        public void Resume()
        {
            //worker task exists, resume from where it was paused
            _resumer.Set();
            //set engine state:
            StateMachineEngine = EngineState.Running;
            RaiseStateMachineSystemEvent("StateMachine: Running", "System running.");
        }

        /// <summary>
        /// ends queue processing
        /// </summary>
        public void Stop()
        {
            //cancel processing
            _tokenSource.Cancel();
            //wait for thread to return:
            _queueWorkerTask.Wait();
            //free resources:
            _queueWorkerTask.Dispose();
            //set engine state:
            StateMachineEngine = EngineState.Stopped;
            RaiseStateMachineSystemEvent("StateMachine: Stopped", "System execution stopped.");
        }

        /// <summary>
        /// Initializes state machine, but does not start it -> dedicated start method
        /// </summary>
        public void InitStateMachine()
        {
            // Set previous state to an unspecific initial state. THe initial state never will be used during normal operation
            PreviousState = _initialState;

            // Look for the default state, which is the state to begin with in StateList.
            foreach (var state in StateList)
            {
                if (state.Value.IsDefaultState)
                {
                    CurrentState = state.Value;
                    //raised in all app services - impl. listener to check that all services have init 
                    RaiseStateMachineSystemCommand("OnInit", "StateMachineInitialized");
                }
            }

            //this is the synchronization object for resuming - passing true means non-blocking (signaled):
            _resumer = new ManualResetEvent(true);
        }

        /// <summary>
        /// worker method for trigger queue
        /// </summary>
        /// <param name="obj"></param>
        private void QueueWorkerMethod(object obj)
        {
            //blocks execution until it's reset.  Used to pause the state machine:
            _resumer.WaitOne();

            //block the queue and loop through all triggers available.  Blocking queue guarantees FIFO and the GetConsumingEnumerable
            //automatically removes triggers from the queue.
            try
            {
                foreach (var trigger in TriggerQueue.GetConsumingEnumerable())
                {
                    if (_tokenSource.IsCancellationRequested)
                    {
                        RaiseStateMachineSystemEvent("StateMachine: QueueWorker", "Processing cancelled!");
                        return;
                    }

                    //compare trigger:
                    foreach (
                        var transition in
                            CurrentState.StateTransitionList.Where(transition => trigger == transition.Value.Trigger))
                    {
                        ExecuteTransition(transition.Value);
                    }
                }
                // Do not place any code here, because it will not be executed!
                // The foreach loop keeps spinning on the queue until thread is canceled.
            }
            catch (Exception exc)
            {
                RaiseStateMachineSystemEvent("StateMachine: QueueWorker", "Processing cancelled! Exception: " + exc);
                Start();
            }
        }

        /// <summary>
        /// Transition to a new state
        /// </summary>
        /// <param name="transition"></param>
        protected virtual void ExecuteTransition(Transition transition)
        {
            //default transition validation
            if (CurrentState.StateName != transition.SourceStateName)
            {
                string message = String.Format("Transition has wrong source state {0}, when system is in {1}",
                    transition.SourceStateName, CurrentState.StateName);
                RaiseStateMachineSystemEvent("StateMachine: Default guard execute transition.", message);
                return;
            }

            if (!StateList.ContainsKey(transition.TargetStateName))
            {
                string message = String.Format("Transition has wrong target state {0}, when system is in {1}. State not in global config.",
                    transition.TargetStateName, CurrentState.StateName);
                RaiseStateMachineSystemEvent("StateMachine: Default guard execute transition.", message);
                return;
            }

            //Self transition - Just do the transition without executing exit, entry actions or guards
            if (transition.SourceStateName == transition.TargetStateName)
            {
                transition.TransitionActionList.ForEach(t => t.Execute());
                return; //Important: Return directly from self-transition
            }

            //run all exit actions of the old state:
            CurrentState.ExitActions.ForEach(a => a.Execute());

            //run all guards of the transition:
            transition.GuardList.ForEach(g => g.Execute());
            string info = transition.GuardList.Count + " guard actios executed.";
            RaiseStateMachineSystemEvent("StateMachine: ExecuteTransition", info);

            //run all actions of the transition:
            transition.TransitionActionList.ForEach(t => t.Execute());

            //IMPORTANT: state change
            info = transition.TransitionActionList.Count + " transition actions executed.";
            RaiseStateMachineSystemEvent("StateMachine: Begin state change...", info);

            //1st resolve the target state with the help of its name:
            var targetState = GetStateFromStateList(transition.TargetStateName);

            //transition successful - change state:
            PreviousState = CurrentState;
            CurrentState = targetState;

            //run all entry actins of the new state:
            foreach (var entryAction in CurrentState.EntryActions)
            {
                entryAction.Execute();
            }
            RaiseStateMachineSystemEvent("StateMachine: State change completed successfully.",
                String.Format("Previous state: {0} - New state: {1}", PreviousState.StateName, CurrentState.StateName));
        }    

        /// <summary>
        /// Enter a trigger into the queue
        /// </summary>
        /// <param name="newTrigger"></param>
        private void EnterTrigger(string newTrigger)
        {
            //put trigger in queue:
            try
            {
                TriggerQueue.Add(newTrigger);
            }
            catch (Exception exc)
            {
                RaiseStateMachineSystemEvent("ActiveStateMachine - error entering trigger", newTrigger + " - " + exc);
            }
            //raise an event:
            RaiseStateMachineSystemEvent("ActiveStateMachine - Trigger entered", newTrigger);
        }

        private State GetStateFromStateList(string targetStateName)
        {
            return StateList[targetStateName];
        }

        #endregion

        #region event infrastructure
        private void RaiseStateMachineSystemEvent(string eventName, string eventInfo)
        {
            if (StateMachineEvent != null) StateMachineEvent(this, new StateMachineEventArgs(eventName, eventInfo, StateMachineEventType.System, "State machine", ""));
        }

        private void RaiseStateMachineSystemCommand(string eventName, string eventInfo)
        {
            if (StateMachineEvent != null) StateMachineEvent(this, new StateMachineEventArgs(eventName, eventInfo, StateMachineEventType.Command, "State machine"));
        }

        /// <summary>
        /// Event handler for internal events triggering the state machine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void InternalNotificationHandler(object sender, StateMachineEventArgs args)
        {
            if (args.EventName == "CompleteFailure")
            {
                RaiseStateMachineSystemCommand("CompleteFailure", args.EventInfo + "Device: " + args.Source);
                //stop state machine to avoid any damage:
                Stop();
            }
            else
            {
                //normal operation:
                EnterTrigger(args.EventName);
            }
        }
        #endregion
    }

    public enum EngineState
    {
        Running,
        Stopped,
        Paused,
        Initialized
    }
}
