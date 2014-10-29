using System;
using UnityEngine;
using Vexe.Editor.Framework;

namespace Vexe.Editor.Framework.GUIs
{
	public partial class GLWrapper
	{
		public Quaternion QuaternionField(Quaternion value)
		{
			return QuaternionField("", value);
		}

		public Quaternion QuaternionField(string label, Quaternion value)
		{
			return QuaternionField(label, value, null);
		}

		public Quaternion QuaternionField(string label, string tooltip, Quaternion value)
		{
			return QuaternionField(label, tooltip, value, null);
		}

		public Quaternion QuaternionField(string label, Quaternion value, params GUILayoutOption[] option)
		{
			return QuaternionField(label, "", value, option);
		}

		public Quaternion QuaternionField(string label, string tooltip, Quaternion value, params GUILayoutOption[] option)
		{
			return QuaternionField(GetContent(label, tooltip), value, option);
		}

		public Quaternion QuaternionField(GUIContent content, Quaternion value, params GUILayoutOption[] option)
		{
			return Quaternion.Euler(Vector3Field(content, value.eulerAngles, option));
		}
	}
}
