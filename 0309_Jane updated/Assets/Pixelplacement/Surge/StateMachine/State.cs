/// <summary>
/// SURGE FRAMEWORK
/// Author: Bob Berkebile
/// Email: bobb@pixelplacement.com
/// 
/// Base class for States to be used as children of StateMachines.
/// 
/// </summary>

using UnityEngine;
using System.Collections;

namespace Pixelplacement
{
	public class State : MonoBehaviour 
	{
		#region Public Properties
		/// <summary>
		/// Gets a value indicating whether this instance is the first state in this state machine.
		/// </summary>
		/// <value><c>true</c> if this instance is first; otherwise, <c>false</c>.</value>
		public bool IsFirst
		{
			get
			{
				return transform.GetSiblingIndex () == 0;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is the last state in this state machine.
		/// </summary>
		/// <value><c>true</c> if this instance is last; otherwise, <c>false</c>.</value>
		public bool IsLast
		{
			get
			{
				return transform.GetSiblingIndex () == transform.parent.childCount - 1;
			}
		}

		/// <summary>
		/// Gets or sets the state machine.
		/// </summary>
		/// <value>The state machine.</value>
		public StateMachine StateMachine
		{
			get;
			set;
		}
		#endregion

		#region Init
		void Awake ()
		{
			if (transform.parent == null) return;
			StateMachine = transform.parent.GetComponent<StateMachine> ();
		}
        #endregion

        #region Public Methods
        /// <summary>
        /// Changes the state.
        /// </summary>
        public void ChangeState(int childIndex)
        {
            StateMachine.ChangeState(childIndex);
        }

        /// <summary>
        /// Changes the state.
        /// </summary>
        public void ChangeState (GameObject state)
		{
			StateMachine.ChangeState (state.name);
		}

		/// <summary>
		/// Changes the state.
		/// </summary>
		public void ChangeState (string state)
		{
			if (StateMachine == null) return;
			StateMachine.ChangeState (state);
		}

		/// <summary>
		/// Change to the next state if possible.
		/// </summary>
		public GameObject Next ()
		{
			return StateMachine.Next ();
		}

		/// <summary>
		/// Change to the previous state if possible.
		/// </summary>
		public GameObject Previous ()
		{
			return StateMachine.Previous ();
		}

		/// <summary>
		/// Exit the current state.
		/// </summary>
		public void Exit ()
		{
			StateMachine.Exit ();
		}
		#endregion
	}
}