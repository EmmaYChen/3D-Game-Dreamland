/// <summary>
/// SURGE FRAMEWORK
/// Author: Bob Berkebile
/// Email: bobb@pixelplacement.com
/// 
/// Custom inspector for the StateMachine class.
/// 
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace Pixelplacement
{
	[CustomEditor (typeof (StateMachine), true)]
	public class StateMachineEditor : Editor 
	{
		#region Private Variables
		StateMachine _target;
		#endregion

		#region Init
		void OnEnable()
		{
			_target = target as StateMachine;
			if (!Application.isPlaying) _target.InitializeStates ();
		}
		#endregion

		#region Inspector GUI
		public override void OnInspectorGUI()
		{
			//if no states are found:
			if (_target.states == null || _target.states.Length < 2)
			{
				DrawNotification("Add child Gameobjects for this State Machine to control.", Color.yellow);
				return;
			}

			//change buttons or default state selection:
			if (EditorApplication.isPlaying)
			{
				DrawStateChangeButtons();
			}
			else
			{
				DrawDefaultStateSelector();
			}

			DrawDefaultInspector ();

			//store new default if one was chosen:
			StoreNewDefaultState ();
		}
		#endregion

		#region GUI Draw Methods
		void DrawStateChangeButtons()
		{
			if (_target.states.Length == 1) return;
			Color currentColor = GUI.color;
			for (int i = 1; i < _target.states.Length; i++)
			{
				if (_target.currentState != null && _target.states[i] == _target.currentState.name) 
				{
					GUI.color = Color.green;
				}else{
					GUI.color = Color.white;
				}

				if (GUILayout.Button (_target.states[i])) _target.ChangeState (_target.states[i]);
			}
			GUI.color = currentColor;
			if (GUILayout.Button ("Exit")) _target.Exit ();
		}

		void DrawDefaultStateSelector ()
		{
			_target.defaultStateIndex = EditorGUILayout.Popup ("Default State on Start:", _target.defaultStateIndex, _target.states);
			DrawHideAllButton ();
			if (!_target.CleanSetup) DrawNotification ("More than one child Gameobject has the same name: Please fix this to allow access to all child states.", Color.yellow);
		}

		void DrawHideAllButton ()
		{
			GUI.color = Color.red;
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Hide All")) 
			{
				Undo.RegisterCompleteObjectUndo (_target.transform, "Hide All");
				foreach (Transform item in _target.transform)
				{
					item.gameObject.SetActive (false);
				}
			}
			GUILayout.EndHorizontal ();
			GUI.color = Color.white;
		}

		void DrawNotification (string message, Color color)
		{
			Color currentColor = GUI.color;
			GUI.color = color;
			EditorGUILayout.HelpBox (message, MessageType.Warning);
			GUI.color = currentColor;
		}
		#endregion

		#region Private Methods
		void StoreNewDefaultState ()
		{
			if (GUI.changed)
			{
				Undo.RecordObject (_target, "Change default state");
				_target.defaultStateName = _target.states [_target.defaultStateIndex];

			}
		}
		#endregion
	}
}