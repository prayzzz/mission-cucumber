using System;
using UnityEngine;

namespace Vexe.Editor.Helpers
{
	[Serializable]
	public class Clipboard : ScriptableObject
	{
		private static Clipboard clipboard;

		[SerializeField]
		private Vector2 vector2Value;

		[SerializeField]
		private Vector3 vector3Value;

		private static string Path
		{
			get { return EditorHelper.ScriptableAssetsPath + "/Clipboard.asset"; }
		}

		private static Clipboard sClipboard
		{
			get { return EditorHelper.LazyLoadScriptableAsset<Clipboard>(ref clipboard, Path, true); }
		}

		public static Vector3 Vector3Value
		{
			get { return sClipboard.vector3Value; }
			set { sClipboard.vector3Value = value; }
		}

		public static Vector2 Vector2Value
		{
			get { return sClipboard.vector2Value; }
			set { sClipboard.vector2Value = value; }
		}
	}
}