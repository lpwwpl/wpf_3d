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
	/// Represents a collection of substates.
	/// </summary>
	public class SubstateCollection : ICollection
	{
        #region SubstateCollection Members

        #region Fields

        // The owner of the collection. The States in the collection are 
        // substates to this State.
        private State owner;

        // The collection of substates.
        private ArrayList substates = new ArrayList();

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the SubstateCollection with the 
        /// specified owner.
        /// </summary>
        /// <param name="owner">
        /// The owner of the collection.
        /// </param>
		public SubstateCollection(State owner)
		{
            this.owner = owner;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the specified State to the collection.
        /// </summary>
        /// <param name="substate">
        /// The State to add to the collection.
        /// </param>
        public void Add(State substate)
        {
            #region Preconditions

            if(owner == substate)
            {
                throw new ArgumentException(
                    "State cannot be a substate to itself.");
            }
            else if(substates.Contains(substate))
            {
                throw new ArgumentException(
                    "State is already a substate to this state.");
            }
            else if(substate.Superstate != null)
            {
                throw new ArgumentException(
                    "State is already a substate to another State.");
            }

            #endregion

            substate.Superstate = owner;
            substates.Add(substate);
        }

        /// <summary>
        /// Removes the specified State from the collection.
        /// </summary>
        /// <param name="substate">
        /// The State to remove from the collection.
        /// </param>
        public void Remove(State substate)
        {
            if(substates.Contains(substate))
            {
                substate.Superstate = null;
                substates.Remove(substate);

                if(owner.InitialState == substate)
                {
                    owner.InitialState = null;
                }
            }
        }

        #endregion

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                return false;               
            }
        }

        public int Count
        {
            get
            {
                return substates.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            substates.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return this;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return substates.GetEnumerator();
        }

        #endregion
    }
}
