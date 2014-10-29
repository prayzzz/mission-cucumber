using UnityEditor;
using UnityEngine;

namespace Vexe.Editor.Framework.GUIs
{
	public partial class GLWrapper
	{
		public int Popup(int selectedIndex, string[] displayedOptions)
		{
			return Popup(selectedIndex, displayedOptions, null);
		}
		public int Popup(int selectedIndex, string[] displayedOptions, params GUILayoutOption[] option)
		{
			return Popup("", selectedIndex, displayedOptions, option);
		}
		public int Popup(string text, int selectedIndex, string[] displayedOptions, params GUILayoutOption[] option)
		{
			return Popup(text, selectedIndex, displayedOptions, EditorStyles.popup, option);
		}
		public int Popup(string text, int selectedIndex, string[] displayedOptions, GUIStyle style, params GUILayoutOption[] option)
		{
			return EditorGUILayout.Popup(text, selectedIndex, displayedOptions, style, option);
		}
	}
}
