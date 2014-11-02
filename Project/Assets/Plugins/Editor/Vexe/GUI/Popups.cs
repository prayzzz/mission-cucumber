using UnityEditor;

using UnityEngine;

namespace Assets.Plugins.Editor.Vexe.GUI
{
    public partial class GLWrapper
    {
        public int Popup(int selectedIndex, string[] displayedOptions)
        {
            return this.Popup(selectedIndex, displayedOptions, null);
        }
        public int Popup(int selectedIndex, string[] displayedOptions, params GUILayoutOption[] option)
        {
            return this.Popup("", selectedIndex, displayedOptions, option);
        }
        public int Popup(string text, int selectedIndex, string[] displayedOptions, params GUILayoutOption[] option)
        {
            return this.Popup(text, selectedIndex, displayedOptions, EditorStyles.popup, option);
        }
        public int Popup(string text, int selectedIndex, string[] displayedOptions, GUIStyle style, params GUILayoutOption[] option)
        {
            return EditorGUILayout.Popup(text, selectedIndex, displayedOptions, style, option);
        }
    }
}
