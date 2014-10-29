﻿using System;
using UnityEditor;
using UnityEngine;
using Vexe.Runtime.Types;

namespace Vexe.Editor.BetterEditors
{
	[CustomEditor(typeof(BetterBehaviour), true)]
	public class BetterBehaviourEditor : BetterEditor
	{
		protected override bool dbg
		{
			get { return GetCustomTypedTarget<BetterBehaviour>().dbg; }
			set { GetCustomTypedTarget<BetterBehaviour>().dbg = value; ; }
		}

		protected override void OnAwakeAssertion()
		{
			if (target is MonoBehaviour && !(target is BetterBehaviour))
			{
				throw new InvalidOperationException("target is a MonoBehaviour but not a BetterBehaviour. Please inherit BetterBehaviour");
			}

			if ((target as BetterBehaviour) == null)
			{
				Debug.LogError(string.Concat(new[] {
								"Casting target object to BetterBehaviour failed! Something's wrong. ",
								"Maybe you switched back and inherited MonoBehaviour instead of BetterBehaviour ",
								"and you still had your gameObject selected? ",
								"If that's the case then the BetterBehaviourEditor is still there in memory ",
								"and so this could be resolved by reselcting your gameObject. ",
								"Destroying this BetterBehaviourEditor instance anyway..."
							}));
				DestroyImmediate(this);
				return;
			}
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
		}
	}
}