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
	/// Represents a collection of Transitions.
	/// </summary>
	public class TransitionCollection : ICollection
	{
        #region TransitionCollection Members

        #region Fields

        // The owner of the collection.
        private State owner = null;

        // The table of transitions.
        private Hashtable transitions = new Hashtable();

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the TransitionCollection class with 
        /// the specified number of events.
        /// </summary>
        /// <param name="owner">
        /// The state that owns the TransitionCollection.
        /// </param>
		public TransitionCollection(State owner)
		{
            this.owner = owner;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a Transition to the collection for the specified event ID.
        /// </summary>
        /// <param name="eventID">
        /// The event ID associated with the Transition.
        /// </param>
        /// <param name="trans">
        /// The Transition to add.
        /// </param>
        /// <remarks>
        /// When a Transition is added to the collection, it is associated with
        /// the specified event ID. When a State receives an event, it looks up
        /// the event ID in its TransitionCollection to see if there are any 
        /// Transitions for the specified event. 
        /// </remarks>
        public void Add(int eventID, Transition trans)
        {
            #region Preconditions

            if(trans.Source != null)
            {
                throw new InvalidOperationException(
                    "This Transition has already been added to another State.");
            }

            #endregion            

            // Set the transition's source.
            trans.Source = owner;

            ArrayList transList;

            // If there are no Transitions for the specified event ID.
            if(transitions[eventID] == null)
            {
                // Create new list of Transitions for the specified event ID.
                transList = new ArrayList();

                transitions.Add(eventID, transList);
            }
            else
            {
                transList = (ArrayList)transitions[eventID];
            }

            // Add Transition.
            transList.Add(trans);
        }

        /// <summary>
        /// Removes the specified Transition at the specified event ID.
        /// </summary>
        /// <param name="eventID">
        /// The event ID associated with the Transition.
        /// </param>
        /// <param name="trans">
        /// The Transition to remove.
        /// </param>
        public void Remove(int eventID, Transition trans)
        {
            // If there are Transitions at the specified event id.
            if(transitions[eventID] != null)
            {
                ArrayList transList = (ArrayList)transitions[eventID];

                transList.Remove(trans);

                // If there are no more Transitions at the specified event id.
                if(transList.Count == 0)
                {
                    // Remove event id.
                    transitions.Remove(eventID);
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a collection of Transitions at the specified event ID.
        /// </summary>
        /// <remarks>
        /// If there are no Transitions at the specified event ID, the value
        /// of the collection will be null.
        /// </remarks>
        public ICollection this[int eventID]
        {
            get
            {
                return (ICollection)transitions[eventID];
            }
        }

        #endregion

        #endregion

        #region ICollection Members

        /// <summary>
        /// Gets a value indicating whether or not the collection is 
        /// synchronized.
        /// </summary>
        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the number of transition tables in the collection.
        /// </summary>
        public int Count
        {
            get
            {
                return transitions.Count;
            }
        }

        /// <summary>
        /// Copies the elements of the collection to an Array, starting at a 
        /// particular Array index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional Array that is the destination of the elements 
        /// copied from ICollection. The Array must have zero-based indexing. 
        /// </param>
        /// <param name="index">
        /// The zero-based index in array at which copying begins. 
        /// </param>
        public void CopyTo(Array array, int index)
        {
            transitions.CopyTo(array, index);
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the 
        /// collection.
        /// </summary>
        public object SyncRoot
        {
            get
            {
                return this;
            }
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that can iterate through a collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can iterate through a collection.
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            return transitions.GetEnumerator();
        }

        #endregion
    }
}
