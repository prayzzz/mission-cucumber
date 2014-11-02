using UnityEditor;

using UnityEngine;

namespace Assets.Plugins.Editor.Vexe.GUI
{
    public partial class GLWrapper
    {
        public Color ColorField(Color value)
        {
            return this.ColorField("", value);
        }
        public Color ColorField(string label, Color value)
        {
            return this.ColorField(label, value, null);
        }
        public Color ColorField(string label, string tooltip, Color value)
        {
            return this.ColorField(label, tooltip, value, null);
        }
        public Color ColorField(string label, Color value, params GUILayoutOption[] option)
        {
            return this.ColorField(label, "", value, option);
        }
        public Color ColorField(string label, string tooltip, Color value, params GUILayoutOption[] option)
        {
            return this.ColorField(GetContent(label, tooltip), value, option);
        }
        public Color ColorField(GUIContent content, Color value, params GUILayoutOption[] option)
        {
            return EditorGUILayout.ColorField(content, value, option);
        }
    }
}
