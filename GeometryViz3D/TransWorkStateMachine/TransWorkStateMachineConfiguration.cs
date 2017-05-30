using System;
using System.Collections.Generic;
using Capiche.ActiveStateMachine;
using Capiche.ApplicationServices;

namespace Statemachine
{
    /// <summary>
    /// Configuration class for telephone state machine sample
    /// The sample shows the inner workings of a state machine and therefore has educational character
    /// A lot of issues have been simplified, for example there are only a few action and no guard actions.
    /// </summary>
    public class TransWorkStateMachineConfiguration
    {

        // Public members
        // List of valid states for this state machine
        public Dictionary<String, State> TransWorkStateMachineStateList { get; set; }
        // List of activities in the system
        public TransWorkActivities TransWorkActivities { get; set; }

        // Max number of entries in trigger queue
        public int MaxEntries { get; set; }
        // Event Manager
        public EventManager TransWorkEventManager;
        // View Manager
        public ViewManager TransWorkViewManager;
        // Device Manager
        //public DeviceManager TelephoneDeviceManager;
        // Logger
        public LogManager TransWorkLogManager;


        /// <summary>
        /// Constructor
        /// </summary>
        public TransWorkStateMachineConfiguration()
        {
            BuildConfig();
        }


        /// <summary>
        /// Build telephone state configuration
        /// </summary>
        private void BuildConfig()
        {
            // Set the maximum queue capacity
            MaxEntries = 50;

            ////////////////////////////////
            // Transitions and actions
            ////////////////////////////////

            // Create the object holding implementation of all system actions
            TransWorkActivities = new TransWorkActivities();

            #region create actions and map action methods into the corresponding action object
            //device actions:
            var ActionIT = new StateMachineAction("ActionIT", TransWorkActivities.ActionIT);
            var ActionViewRotate_X = new StateMachineAction("ActionViewRotate_X", TransWorkActivities.ActionViewRotate_X);
            var ActionViewRotate_Z = new StateMachineAction("ActionViewRotate_Z", TransWorkActivities.ActionViewRotate_Z);
            var ActionTranslate = new StateMachineAction("ActionTranslate", TransWorkActivities.ActionTranslate);
            var ActionViewTranslateX = new StateMachineAction("ActionViewTranslateX", TransWorkActivities.ActionViewTranslateX);
            var ActionViewTranslateY = new StateMachineAction("ActionViewTranslateY", TransWorkActivities.ActionViewTranslateY);
            var ActionViewTranslateZ = new StateMachineAction("ActionViewTranslateZ", TransWorkActivities.ActionViewTranslateZ);
            var ActionEnd = new StateMachineAction("ActionEnd", TransWorkActivities.ActionEnd);

            // Error action
            var ActionError = new StateMachineAction("ActionError", TransWorkActivities.ActionError);


            //  Create transitions and corresponding triggers, states need to be added. 
            var emptyList = new List<StateMachineAction>(); 

            var ITActions = new List<StateMachineAction>();
            ITActions.Add(ActionIT);
            var transIncomingTrans = new Transition("TransitionIncomingTrans", "StateIdle", "StateRotate", emptyList, ITActions, "OnTransStart");

            var RotateXActions = new List<StateMachineAction>();
            RotateXActions.Add(ActionViewRotate_X);
            var transRotateX = new Transition("TransitionRotateX", "StateRotate", "StateRotate", emptyList, RotateXActions, "OnRotateXActive");


            var RotateZActions = new List<StateMachineAction>();
            RotateZActions.Add(ActionViewRotate_Z);
            var transRotateZ = new Transition("TransitionRotateZ", "StateRotate", "StateRotate", emptyList, RotateZActions, "OnRotateZActive");//source & target both 'StatePhoneRings'


            var OffsetActions = new List<StateMachineAction>();
            OffsetActions.Add(ActionTranslate); // Go back to Phone Idle state
            var transOffset = new Transition("TransitionOffset", "StateRotate", "StateOffset", emptyList, OffsetActions, "OnOffsetActive");

            var OffsetXActions = new List<StateMachineAction>();
            OffsetXActions.Add(ActionViewTranslateX); // Go back to Phone Idle state
            var transOffsetX = new Transition("TransitionOffsetX", "StateOffset", "StateOffset", emptyList, OffsetXActions, "OnOffsetXActive");

            var OffsetYActions = new List<StateMachineAction>();
            OffsetYActions.Add(ActionViewTranslateY); // Go back to Phone Idle state
            var transOffsetY = new Transition("TransitionOffsetY", "StateOffset", "StateOffset", emptyList, OffsetYActions, "OnOffsetYActive");

            var OffsetZActions = new List<StateMachineAction>();
            OffsetZActions.Add(ActionViewTranslateZ); // Go back to Phone Idle state
            var transOffsetZ = new Transition("TransitionOffsetZ", "StateOffset", "StateOffset", emptyList, OffsetZActions, "OnOffsetZActive");

            var EndActions = new List<StateMachineAction>();
            EndActions.Add(ActionEnd); // Go back to Phone Idle state
            var transEnd = new Transition("TransitionEnd", "StateOffset", "StateIdle", emptyList, EndActions, "OnEnd");
            #endregion

            #region States Assemble!
            var transitionsIdle = new Dictionary<String, Transition>();
            var entryActionsIdle = new List<StateMachineAction>();
            var exitActionsIdle = new List<StateMachineAction>();
            transitionsIdle.Add("TransitionIT", transIncomingTrans);
            // Always specify all action lists, even empty ones, do not pass null into a state -> Lists are read via foreach, which will return an error, if they are null!
            var idle = new State("StateIdle", transitionsIdle, entryActionsIdle, exitActionsIdle, true);


            var entryActionsRotate = new List<StateMachineAction>();
            var exitActionsRotate = new List<StateMachineAction>();
            //transitions
            var transitionsRotate = new Dictionary<String, Transition>();
            transitionsRotate.Add("TransitionRotateX", transRotateX);
            transitionsRotate.Add("TransitionRotateZ", transRotateZ);
            transitionsRotate.Add("TransitionOffset", transOffset);
            var rotate = new State("StateRotate", transitionsRotate, entryActionsRotate, exitActionsRotate);


            var entryActionsOffset = new List<StateMachineAction>();
            var exitActionsOffset = new List<StateMachineAction>();
            //transitions
            var transitionsOffset = new Dictionary<String, Transition>();
            transitionsOffset.Add("TransitionOffsetX", transOffsetX);
            transitionsOffset.Add("TransitionOffsetY", transOffsetY);
            transitionsOffset.Add("TransitionOffsetZ", transOffsetZ);
            transitionsOffset.Add("TransitionEnd", transEnd);
            var offset = new State("StateOffset", transitionsOffset, entryActionsOffset, exitActionsOffset);

            TransWorkStateMachineStateList = new Dictionary<string, State>
            {
                {"StateIdle", idle},
                {"StateRotate", rotate},
                {"StateOffset", offset},
            };

            #endregion

            #region Application Services
            TransWorkEventManager = EventManager.Instance;
            TransWorkViewManager = ViewManager.Instance;
            TransWorkLogManager = LogManager.Instance;
            //TelephoneDeviceManager = DeviceManager.Instance;
            #endregion
        }

        public void DoEventMappings(TransWorkStateMachine transWorkStateMachine, TransWorkActivities transWorkActivities)
        {
            #region register events
            //use case impl.
            TransWorkEventManager.RegisterEvent("TransUIEvent", transWorkActivities);
            TransWorkEventManager.RegisterEvent("ViewManagerEvent", TransWorkViewManager);
            TransWorkEventManager.RegisterEvent("EventManagerEvent", TransWorkEventManager);
            TransWorkEventManager.RegisterEvent("StateMachineEvent", transWorkStateMachine);
            #endregion

            //subscribe event handlers to registered with the event manager
            #region event mappings
            //logging
            TransWorkEventManager.SubscribeEvent("ViewManagerEvent", "LogEventHandler", TransWorkLogManager);
            TransWorkEventManager.SubscribeEvent("EventManagerEvent", "LogEventHandler", TransWorkLogManager);
            TransWorkEventManager.SubscribeEvent("StateMachineEvent", "LogEventHandler", TransWorkLogManager);

            //system event listeners in managers
            //TransWorkEventManager.SubscribeEvent("TransUIEvent", "ViewCommandHandler", TransWorkEventManager);
            //TransWorkEventManager.SubscribeEvent("StateMachineEvent", "SystemEventHandler", TransWorkEventManager);
            //TransWorkEventManager.SubscribeEvent("ViewManagerEvent", "DeviceCommandHandler", TelephoneDeviceManager);
            #endregion
        }
    }
}
