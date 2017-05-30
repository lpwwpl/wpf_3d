/*
 * Created by: Leslie Sanford
 * 
 * Contact: jabberdabber@hotmail.com
 * 
 * Last modified: 10/01/2005
 */

using System;
using System.CodeDom;
using System.Collections;

namespace Sanford.StateMachineToolkit
{
	/// <summary>
	/// Builds the method responsible for initializing the states.
	/// </summary>
    internal class StateInitializeBuilder
	{
        #region StateInitializeBuilder Members

        #region Fields

        // The state machine's states.
        private ICollection states;

        // The state machine's events.
        private ICollection events;

        // The built method.
        private CodeMemberMethod result = new CodeMemberMethod();

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the StateInitializeBuilder class with
        /// specified state and event tables.
        /// </summary>
        /// <param name="states">
        /// The states to be initialized.
        /// </param>
        /// <param name="events">
        /// The  events the state machine responds to.
        /// </param>
		public StateInitializeBuilder(ICollection states, ICollection events)
		{
            this.states = states;
            this.events = events;
		}

        #endregion

        #region Methods

        /// <summary>
        /// Builds the method.
        /// </summary>
        public void Build()
        {
            result = new CodeMemberMethod();
            result.Name = "InitializeStates";
            result.Attributes = MemberAttributes.Private;

            CodeThisReferenceExpression thisReference = new CodeThisReferenceExpression();
            CodeFieldReferenceExpression stateField;
            CodeDelegateCreateExpression delegateCreate;
            CodeVariableDeclarationStatement delegateVariable;  
            CodeObjectCreateExpression stateCreate;
            CodeTypeReference enumReference = new CodeTypeReference("StateID");

            foreach(string name in states)
            {
                delegateCreate = 
                    new CodeDelegateCreateExpression(
                    new CodeTypeReference(typeof(EntryHandler)), 
                    thisReference, "Entry" + name);
                delegateVariable = new CodeVariableDeclarationStatement(
                    typeof(EntryHandler), "en" + name, delegateCreate);

                result.Statements.Add(delegateVariable);

                delegateCreate = 
                    new CodeDelegateCreateExpression(
                    new CodeTypeReference(typeof(ExitHandler)), 
                    thisReference, "Exit" + name);
                delegateVariable = new CodeVariableDeclarationStatement(
                    typeof(ExitHandler), "ex" + name, delegateCreate);

                result.Statements.Add(delegateVariable);   
             
                stateField = new CodeFieldReferenceExpression(thisReference, 
                    "state" + name);  

                CodeFieldReferenceExpression enumFieldReference = new CodeFieldReferenceExpression(
                    new CodeTypeReferenceExpression("StateID"), name);

                System.CodeDom.CodeCastExpression enumCast = 
                    new CodeCastExpression(typeof(int), enumFieldReference);

                CodeExpression[] parameters = 
                    {              
                        enumCast,
                        new CodeVariableReferenceExpression("en" + name),
                        new CodeVariableReferenceExpression("ex" + name)
                    };

                stateCreate = new CodeObjectCreateExpression(typeof(State), parameters);

                result.Statements.Add(new CodeAssignStatement(stateField, stateCreate));
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the build method.
        /// </summary>
        public CodeMemberMethod Result
        {
            get
            {
                return result;
            }
        }

        #endregion

        #endregion
	}
}
