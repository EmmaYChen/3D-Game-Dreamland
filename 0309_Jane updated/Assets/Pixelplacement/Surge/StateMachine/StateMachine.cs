/// <summary>
/// SURGE FRAMEWORK
/// Author: Bob Berkebile
/// Email: bobb@pixelplacement.com
/// </summary>

// Used to disable the lack of usage of the exception in a try/catch:
#pragma warning disable 168

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

namespace Pixelplacement
{
	[RequireComponent (typeof (Initialization))]
	public class StateMachine : MonoBehaviour 
	{
		#region Public Variables
		[HideInInspector][SerializeField] public string[] states;
		[HideInInspector][SerializeField] public int defaultStateIndex;
		[HideInInspector][SerializeField] public string defaultStateName = "None";
		[HideInInspector][SerializeField] public GameObject currentState;

		/// <summary>
		/// Should log messages be thrown during usage?
		/// </summary>
		[Tooltip("Should log messages be thrown during usage?")]
		public bool verbose = true;

		/// <summary>
		/// Can States within this StateMachine be reentered?
		/// </summary>
		[Tooltip("Can States within this StateMachine be reentered?")]
		public bool allowReentry = false;

		/// <summary>
		/// Return to default state on disable?
		/// </summary>
		[Tooltip("Return to default state on disable?")]
		public bool returnToDefaultOnDisable = true;

		public Dictionary<string, GameObject> StateLinks { private get; set; }
		#endregion

		#region Publice Events
		public GameObjectEvent OnStateExited;
		public GameObjectEvent OnStateEntered;
		public UnityEvent OnFirstStateEntered;
		public UnityEvent OnFirstStateExited;
		public UnityEvent OnLastStateEntered;
		public UnityEvent OnLastStateExited;
		#endregion

		#region Public Properties
		/// <summary>
		/// Internal flag used to determine if the StateMachine is set up properly.
		/// </summary>
		/// <value><c>true</c> if clean setup; otherwise, <c>false</c>.</value>
		public bool CleanSetup 
		{ 
			get; 
			private set; 
		}

		/// <summary>
		/// Are we at the first state in this state machine.
		/// </summary>
		/// <value><c>true</c> if at first; otherwise, <c>false</c>.</value>
		public bool AtFirst
		{
			get
			{
				return _atFirst;
			}

			private set
			{
				if (_atFirst)
				{
					_atFirst = false;
					if (OnFirstStateExited != null) OnFirstStateExited.Invoke ();
				} else {
					_atFirst = true;
					if (OnFirstStateEntered != null) OnFirstStateEntered.Invoke ();
				}
			}
		}

		/// <summary>
		/// Are we at the last state in this state machine.
		/// </summary>
		/// <value><c>true</c> if at last; otherwise, <c>false</c>.</value>
		public bool AtLast
		{
			get
			{
				return _atLast;
			}

			private set
			{
				if (_atLast)
				{
					_atLast = false;
					if (OnLastStateExited != null) OnLastStateExited.Invoke ();
				} else {
					_atLast = true;
					if (OnLastStateEntered != null) OnLastStateEntered.Invoke ();
				}
			}
		}
		#endregion

		#region Private Variables
		bool _initialized;
		bool _atFirst;
		bool _atLast;
		#endregion

		#region Public Methods
		/// <summary>
		/// Change to the next state if possible.
		/// </summary>
		public GameObject Next ()
		{
			if (currentState == null) return (ChangeState (states[1]));	

			int currentIndex = -1;
			for (int i = 0; i < states.Length; i++)
			{
				if (states[i] == currentState.name)
				{
					currentIndex = i;
					break;
				}
			}

			if (currentIndex == states.Length - 1)
			{
				return currentState;	
			}else{
				return ChangeState (states [++currentIndex]);
			}
		}

		/// <summary>
		/// Change to the previous state if possible.
		/// </summary>
		public GameObject Previous ()
		{
			if (currentState == null) return (ChangeState (states[1]));	

			int currentIndex = -1;
			for (int i = 0; i < states.Length; i++)
			{
				if (states[i] == currentState.name)
				{
					currentIndex = i;
					break;
				}
			}

			if (currentIndex == 1)
			{
				return currentState;	
			}else{
				return ChangeState (states [--currentIndex]);
			}
		}

		/// <summary>
		/// Exit the current state.
		/// </summary>
		public void Exit ()
		{
			if (currentState == null) return;
			Log ("(-) " + name + " EXITED state: " + currentState.name);
			int index = currentState.transform.GetSiblingIndex ();

			//no longer at first:
			if (index == 0)
			{
				AtFirst = false;
			}

			//no longer at last:
			if (index == transform.childCount - 1)
			{
				AtLast = false;	
			}

			if (OnStateExited != null) OnStateExited.Invoke (currentState);
			currentState.SetActive (false);
			currentState = null;
		}

        /// <summary>
		/// Changes the state.
		/// </summary>
		public GameObject ChangeState(int childIndex)
        {
            return ChangeState(transform.GetChild(childIndex).gameObject);
        }

        /// <summary>
        /// Changes the state.
        /// </summary>
        public GameObject ChangeState (GameObject state)
		{
			return ChangeState (state.name);
		}

		/// <summary>
		/// Changes the state.
		/// </summary>
		public GameObject ChangeState (string state)
		{
			if (!StateLinks.ContainsKey(state))
			{
				Log ("\"" + name + "\" does not contain a state by the name of \"" + state + "\" please verify the name of the state you are trying to reach.");
				return null;
			}

			if (currentState != null) 
			{
				if (!allowReentry && state == currentState.name ) 
				{
					Log ("State change ignored. State machine \"" + name + "\" already in \"" + state + "\" state.");
					return null;
				}
			}

			Exit ();
			Enter (state);

			return currentState;
		}

		/// <summary>
		/// Internally used within the framework to ensure proper order of operations.
		/// </summary>
		public void InitializeStates ()
		{
			//preset cleanup status:
			CleanSetup = true;

			//look for children:
			Dictionary<string, GameObject> foundStates = new Dictionary<string, GameObject> ();

			//cycle children to find states:
			for (int i = 0; i < transform.childCount; i++) 
			{
				Transform current = transform.GetChild (i);

				if (Application.isPlaying) current.gameObject.SetActive (false);
				try 
				{
					//catalog:
					foundStates.Add (current.name, current.gameObject);
					State currentState = current.gameObject.GetComponent<State> ();
					if (currentState != null) currentState.StateMachine = this;
				} catch (System.Exception ex) {
					CleanSetup = false;
				}
			}

			//create states array and inject "None" into it:
			StateLinks = foundStates;
			states = new string[foundStates.Count + 1];
			foundStates.Keys.CopyTo (states, 1);
			states [0] = "None";

			//if the default state was deleted:
			if (defaultStateIndex > states.Length - 1 || defaultStateName != states[defaultStateIndex]) 
			{
				#if UNITY_EDITOR
				EditorUtilities.Error ("Default state \"" + defaultStateName + "\" for State Machine: \"" + gameObject.name + "\" was not found or a state was added or deleted - you will need to reset your default state.");
				#endif
				defaultStateIndex = 0;
				defaultStateName = "None";
			}
		}

		/// <summary>
		/// Internally used within the framework to auto start the state machine.
		/// </summary>
		public void StartMachine ()
		{
			//start the machine:
			if (Application.isPlaying && defaultStateIndex > 0) ChangeState (defaultStateName);
		}
		#endregion

		#region Private Methods
		void Enter (string state)
		{
			currentState = StateLinks [state];
			int index = currentState.transform.GetSiblingIndex ();

			//entering first:
			if (index == 0)
			{
				AtFirst = true;
			}

			//entering last:
			if (index == transform.childCount - 1)
			{
				AtLast = true;	
			}

			Log( "(+) " + name + " ENTERED state: " + state );
			if (OnStateEntered != null) OnStateEntered.Invoke (currentState);
			currentState.SetActive (true);
		}

		void Log (string message)
		{
			if (!verbose) return;
			Debug.Log (message, gameObject);
		}
		#endregion
	}
}