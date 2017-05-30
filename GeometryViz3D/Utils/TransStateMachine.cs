using Sanford.Threading;
using System.Threading;
using Sanford.StateMachineToolkit;
using System;
using System.ComponentModel;
using GeometryViz3D.ViewModels;
using System.Windows;

namespace GeometryViz3D.StateMachine
{
    public class MyEventArgs : EventArgs
    {
        public double value = 0;
        public int eventId = 0;
        public MyEventArgs(int _eventId, double _value)
        {
            eventId = _eventId;
            value = _value;
        }
    }

    public class TransStateMachine : PassiveStateMachine
    {
        public enum EventID
        {

            RotateX = 0,

            RotateZ,

            Offset,

            OffsetX,

            OffsetY,

            OffsetZ,

            Reset,

            End,
        }

        public enum StateID
        {

            StateIdle,

            StateRotate,

            StateOffset,

            StateEnd
        }
        private Sanford.StateMachineToolkit.State stateStateIdle;
        
        private Sanford.StateMachineToolkit.State stateStateRotate;
        
        private Sanford.StateMachineToolkit.State stateStateOffset;

        private Sanford.StateMachineToolkit.State stateStateEnd;
        
        public TransStateMachine()
        {
            this.Initialize();
        }
        
        private void Initialize()
        {
            this.InitializeStates();
            this.InitializeGuards();
            this.InitializeActions();
            this.InitializeTransitions();
            this.InitializeRelationships();
            this.InitializeHistoryTypes();
            this.InitializeInitialStates();
            this.Initialize(this.stateStateIdle);
        }
        
        private void InitializeStates()
        {
            Sanford.StateMachineToolkit.EntryHandler enStateIdle = new Sanford.StateMachineToolkit.EntryHandler(this.EntryStateIdle);
            Sanford.StateMachineToolkit.ExitHandler exStateIdle = new Sanford.StateMachineToolkit.ExitHandler(this.ExitStateIdle);
            this.stateStateIdle = new Sanford.StateMachineToolkit.State(((int)(StateID.StateIdle)), enStateIdle, exStateIdle);
            Sanford.StateMachineToolkit.EntryHandler enStateRotate = new Sanford.StateMachineToolkit.EntryHandler(this.EntryStateRotate);
            Sanford.StateMachineToolkit.ExitHandler exStateRotate = new Sanford.StateMachineToolkit.ExitHandler(this.ExitStateRotate);
            this.stateStateRotate = new Sanford.StateMachineToolkit.State(((int)(StateID.StateRotate)), enStateRotate, exStateRotate);
            Sanford.StateMachineToolkit.EntryHandler enStateOffset = new Sanford.StateMachineToolkit.EntryHandler(this.EntryStateOffset);
            Sanford.StateMachineToolkit.ExitHandler exStateOffset = new Sanford.StateMachineToolkit.ExitHandler(this.ExitStateOffset);
            this.stateStateOffset = new Sanford.StateMachineToolkit.State(((int)(StateID.StateOffset)), enStateOffset, exStateOffset);
            Sanford.StateMachineToolkit.EntryHandler enStateEnd = new Sanford.StateMachineToolkit.EntryHandler(this.EntryStateEnd);
            Sanford.StateMachineToolkit.ExitHandler exStateEnd = new Sanford.StateMachineToolkit.ExitHandler(this.ExitStateEnd);
            this.stateStateEnd = new Sanford.StateMachineToolkit.State(((int)(StateID.StateEnd)), enStateEnd, exStateEnd);
        }
        
        private void InitializeGuards()
        {
        }
        
        private void InitializeActions()
        {
        }
        protected G3DViewportViewModel model = null;
        public void SetVMCtrl(G3DViewportViewModel _model)
        {
            model = _model;
        }
        private void InitializeTransitions()
        {

            ////////////////////idle state-->Rotate
            Sanford.StateMachineToolkit.Transition trans;
            trans = new Sanford.StateMachineToolkit.Transition(null, this.stateStateRotate);
            trans.Actions.Add(new ActionHandler(Rotate));
            this.stateStateIdle.Transitions.Add(((int)(EventID.RotateX)), trans);

            trans = new Sanford.StateMachineToolkit.Transition(null, this.stateStateRotate);
            trans.Actions.Add(new ActionHandler(Rotate));
            this.stateStateIdle.Transitions.Add(((int)(EventID.RotateZ)), trans);

            ////////////////////Rotate state-->Offset, Rotate-->Idle
            trans = new Sanford.StateMachineToolkit.Transition(null, this.stateStateRotate);
            trans.Actions.Add(new ActionHandler(Rotate));
            this.stateStateRotate.Transitions.Add(((int)(EventID.RotateX)), trans);

            trans = new Sanford.StateMachineToolkit.Transition(null, this.stateStateRotate);
            trans.Actions.Add(new ActionHandler(Rotate));
            this.stateStateRotate.Transitions.Add(((int)(EventID.RotateZ)), trans);

            trans = new Sanford.StateMachineToolkit.Transition(null, this.stateStateIdle);
            trans.Actions.Add(new ActionHandler(Reset));
            this.stateStateRotate.Transitions.Add(((int)(EventID.Reset)), trans);

            trans = new Sanford.StateMachineToolkit.Transition(null, this.stateStateOffset);
            //trans.Actions.Add(new ActionHandler(Offset));
            this.stateStateRotate.Transitions.Add(((int)(EventID.Offset)), trans);


            ////////////////////Offset state-->End, Offset-->Idle
            trans = new Sanford.StateMachineToolkit.Transition(null, this.stateStateOffset);
            trans.Actions.Add(new ActionHandler(Offset));
            this.stateStateOffset.Transitions.Add(((int)(EventID.OffsetX)), trans);

            trans = new Sanford.StateMachineToolkit.Transition(null, this.stateStateOffset);
            trans.Actions.Add(new ActionHandler(Offset));
            this.stateStateOffset.Transitions.Add(((int)(EventID.OffsetY)), trans);

            trans = new Sanford.StateMachineToolkit.Transition(null, this.stateStateOffset);
            trans.Actions.Add(new ActionHandler(Offset));
            this.stateStateOffset.Transitions.Add(((int)(EventID.OffsetZ)), trans);

            trans = new Sanford.StateMachineToolkit.Transition(null, this.stateStateIdle);
            trans.Actions.Add(new ActionHandler(Reset));
            this.stateStateOffset.Transitions.Add(((int)(EventID.Reset)), trans);

            trans = new Sanford.StateMachineToolkit.Transition(null, this.stateStateEnd);
            trans.Actions.Add(new ActionHandler(End));
            this.stateStateOffset.Transitions.Add(((int)(EventID.End)), trans);

            ///////End-->Idle
            trans = new Sanford.StateMachineToolkit.Transition(null, this.stateStateIdle);
            trans.Actions.Add(new ActionHandler(Reset));
            this.stateStateEnd.Transitions.Add(((int)(EventID.Reset)), trans);
        }
        private void Rotate(object[] args)
        {
            model.Rotate((MyEventArgs)args[0]);
        }

        private void Offset(object[] args)
        {
            model.Offset((MyEventArgs)args[0]);
        }
        private void Reset(object[] args)
        {
            model.Reset((MyEventArgs)args[0]);
            model.RotateVisibility = Visibility.Visible;
        }
        private void End(object[] args)
        {
            Console.Write("");
        }

        private void InitializeRelationships()
        {
        }
        
        private void InitializeHistoryTypes()
        {
            this.stateStateIdle.HistoryType = Sanford.StateMachineToolkit.HistoryType.None;
            this.stateStateOffset.HistoryType = Sanford.StateMachineToolkit.HistoryType.None;
            this.stateStateRotate.HistoryType = Sanford.StateMachineToolkit.HistoryType.None;
            this.stateStateEnd.HistoryType = Sanford.StateMachineToolkit.HistoryType.None;
        }
        
        private void InitializeInitialStates()
        {
        }        
        protected virtual void EntryStateIdle()
        {
        }        
        protected virtual void EntryStateRotate()
        {
        }        
        protected virtual void EntryStateOffset()
        {
        }
        protected virtual void EntryStateEnd()
        {
        }
        protected virtual void ExitStateIdle()
        {
        }        
        protected virtual void ExitStateRotate()
        {
        }        
        protected virtual void ExitStateOffset()
        {
        }
        protected virtual void ExitStateEnd()
        {
        }
    }
}
