using System;
using Capiche.Common;

namespace Statemachine
{
    public class TransWorkActivities
    {
        //events to communicate from state machine to managers - wiring will be done via the event manager
        public event EventHandler<StateMachineEventArgs> TransEvent; 

        #region view actions
        public void ActionViewRotate_X()
        {
            RaiseTransEventEvent("ViewRotate_X");
        }
        public void ActionTranslate()
        {
            RaiseTransEventEvent("ViewTranslate");
        }
        public void ActionEnd()
        {
            RaiseTransEventEvent("ViewEnd");
        }
        public void ActionIT()
        {
            RaiseTransEventEvent("ViewIT");
        }
        public void ActionViewRotate_Z()
        {
            RaiseTransEventEvent("ViewRotate_Z");
        }
        public void ActionViewTranslateX()
        {
            RaiseTransEventEvent("ViewTranslate_X");
        }
        public void ActionViewTranslateY()
        {
            RaiseTransEventEvent("ViewTranslate_Y");
        }
        public void ActionViewTranslateZ()
        {
            RaiseTransEventEvent("ViewTranslate_Z");
        }
        public void ActionError()
        {
            RaiseTransEventEvent("ViewError");
        }
        #endregion

        #region event methods
        private void RaiseTransEventEvent(string command)
        {
            var teleArgs = new StateMachineEventArgs(command, "Trans command", StateMachineEventType.Command, "State machine action", "ViewManager");
            TransEvent(this, teleArgs);
        }
        #endregion
    }
}
