using Assets.Plugins.Editor.Vexe.Other;

using UnityEditor;

using UnityEngine;

namespace Assets.Plugins.Editor.Vexe.GUI
{
    public partial class GLWrapper
    {
        public Rect RectField(Rect value)
        {
            return this.RectField("", value);
        }
        public Rect RectField(string label, Rect value)
        {
            return this.RectField(label, value, GUIHelper.MultifieldOption);
        }
        public Rect RectField(string label, string tooltip, Rect value)
        {
            return this.RectField(label, tooltip, value, GUIHelper.MultifieldOption);
        }
        public Rect RectField(string label, Rect value, params GUILayoutOption[] option)
        {
            return this.RectField(label, "", value, option);
        }
        public Rect RectField(string label, string tooltip, Rect value, params GUILayoutOption[] option)
        {
            return this.RectField(new GUIContent(label, tooltip), value, option);
        }
        public Rect RectField(GUIContent content, Rect value, params GUILayoutOption[] option)
        {
            return EditorGUILayout.RectField(content, value, option);
        }
    }
}
