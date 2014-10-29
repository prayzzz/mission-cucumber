using System;
using System.Reflection;
using Fasterflect;
using UnityEngine;
using Vexe.Editor.Framework.GUIs;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Vexe.Editor.Framework.Drawers
{
	public class MethodDrawer : BaseDrawer
	{
		private MethodInfo method;
		private DataInfo[] argMembers;
		private object[] argValues;
		private MethodInvoker invoke;

		protected override void OnInitialized()
		{
			method       = memberInfo as MethodInfo;
			invoke	    = method.DelegateForCallMethod();
			var argInfos = method.GetParameters();
			int len      = argInfos.Length;
			argValues    = new object[len];
			argMembers   = new DataInfo[len];

			for (int i = 0; i < len; i++)
			{
				var arg = argInfos[i];
				var argType = arg.ParameterType;

				argValues[i] = argInfos[i].ParameterType.GetDefaultValueEmptyIfString();

				argMembers[i] = new DataInfo(
						@id            : id + argType.FullName + arg.Name,
						@getter        : () => argValues[i],
						@setter        : x => argValues[i] = x,
						@attributes    : new Attribute[0],
						@name          : arg.Name,
						@elementType   : argType,
						@declaringType : targetType,
						@reflectedType : GetType()
					);
			}

			dbgLog("Method drawer init");
		}

		public override void OnGUI()
		{
			if (!Header())
				return;

			gui._beginIndent();
			{
				for (int i = 0; i < argMembers.Length; i++)
				{
					gui.MemberField(argMembers[i], rawTarget, unityTarget, id);
				}
			}
			gui._endIndent();
		}

		private bool Header()
		{
			gui._beginH();
			{
				if (argMembers.Length == 0)
					gui.Label(niceName);
				else
					foldout = gui.Foldout(niceName, foldout, GUILayout.ExpandWidth(true));
				gui.FlexibleSpace();
				if (gui.MiniButton("Invoke", MiniButtonStyle.Right, null))
					invoke(rawTarget, argValues);
			}
			gui._endH();
			return argMembers.Length == 0 || foldout;
		}
	}
}