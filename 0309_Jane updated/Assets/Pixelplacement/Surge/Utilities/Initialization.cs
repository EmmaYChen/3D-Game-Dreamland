/// <summary>
/// SURGE FRAMEWORK
/// Author: Bob Berkebile
/// Email: bobb@pixelplacement.com
/// 
/// Utilizes script execution order to run before anything else to avoid order of operation failures so accessing things like singletons at any stage of application startup will never fail.
/// 
/// </summary>

using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

namespace Pixelplacement
{
	public class Initialization : MonoBehaviour 
	{
		#region Private Variables
		StateMachine _stateMachine;
		DisplayObject _displayObject;
		#endregion

		#region Init
		void Awake () 
		{
			//singleton initialization:
			InitializeSingleton ();

			//values:
			_stateMachine = GetComponent<StateMachine> ();
			_displayObject = GetComponent<DisplayObject> ();

			//display object initialization:
			if (_displayObject != null) _displayObject.Register ();

			//state machine initialization:
			if (_stateMachine != null) _stateMachine.InitializeStates ();
		}

		void Start ()
		{
			//state machine start:
			if (_stateMachine != null) _stateMachine.StartMachine ();
		}
		#endregion

		#region Deinit
		void OnDisable ()
		{
			if (_stateMachine != null)
			{
				if (!_stateMachine.returnToDefaultOnDisable || _stateMachine.defaultStateName == "None") return;

				if (_stateMachine.currentState == null)
				{
					_stateMachine.ChangeState (_stateMachine.defaultStateName);
					return;
				}

				if (_stateMachine.currentState.name != _stateMachine.defaultStateName)
				{
					_stateMachine.ChangeState (_stateMachine.defaultStateName);
				}
			}
		}
		#endregion

		#region Private Methods
		void InitializeSingleton ()
		{
			foreach (Component item in GetComponents<Component> ()) 
			{
				string baseType;

				#if NETFX_CORE
				baseType = item.GetType ().GetTypeInfo ().BaseType.ToString ();
				#else
				baseType = item.GetType ().BaseType.ToString ();
				#endif

				if (baseType.Contains ("Singleton") && baseType.Contains ("Pixelplacement"))
				{
					MethodInfo m;

					#if NETFX_CORE
					m = item.GetType ().GetTypeInfo ().BaseType.GetMethod ("Initialize", BindingFlags.NonPublic | BindingFlags.Instance);
					#else
					m = item.GetType ().BaseType.GetMethod ("Initialize", BindingFlags.NonPublic | BindingFlags.Instance);
					#endif

					m.Invoke (item, new Component[] {item});
					break;
				}
			}
		}
		#endregion
	}
}