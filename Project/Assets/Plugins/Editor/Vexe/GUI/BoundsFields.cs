using Assets.Plugins.Editor.Vexe.Other;

using UnityEditor;

using UnityEngine;

namespace Assets.Plugins.Editor.Vexe.GUI
{
    public partial class GLWrapper
    {
        public Bounds BoundsField(Bounds value)
        {
            return this.BoundsField("", value);
        }
        public Bounds BoundsField(string label, Bounds value)
        {
            return this.BoundsField(label, value, GUIHelper.MultifieldOption);
        }
        public Bounds BoundsField(string label, string tooltip, Bounds value)
        {
            return this.BoundsField(label, tooltip, value, GUIHelper.MultifieldOption);
        }
        public Bounds BoundsField(string label, Bounds value, params GUILayoutOption[] option)
        {
            return this.BoundsField(label, "", value, option);
        }
        public Bounds BoundsField(string label, string tooltip, Bounds value, params GUILayoutOption[] option)
        {
            return this.BoundsField(GetContent(label, tooltip), value, option);
        }
        public Bounds BoundsField(GUIContent content, Bounds value, params GUILayoutOption[] option)
        {
            return EditorGUILayout.BoundsField(content, value, option);
        }
    }
}