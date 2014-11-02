using Assets.Plugins.Editor.Vexe.Other;

using UnityEditor;

using UnityEngine;

namespace Assets.Plugins.Editor.Vexe.GUI
{
    public partial class GLWrapper
    {
        public bool Foldout(bool foldout)
        {
            return this.Foldout("", foldout);
        }

        public bool Foldout(bool foldout, params GUILayoutOption[] option)
        {
            return this.Foldout("", foldout, option);
        }

        public bool Foldout(string label, bool foldout)
        {
            return this.Foldout(label, foldout, EditorStyles.foldout);
        }

        public bool Foldout(string label, bool foldout, GUIStyle style)
        {
            return this.Foldout(label, foldout, "", style, GUILayout.Width(GUIConstants.kFoldoutWidth));
        }

        public bool Foldout(string label, bool foldout, params GUILayoutOption[] option)
        {
            return this.Foldout(label, foldout, "", EditorStyles.foldout, option);
        }

        public bool Foldout(string label, bool foldout, GUIStyle style, params GUILayoutOption[] option)
        {
            return this.Foldout(label, foldout, "", style, option);
        }

        public bool Foldout(string label, bool foldout, string tooltip, GUIStyle style, params GUILayoutOption[] option)
        {
            return this.Foldout(GetContent(label, tooltip), foldout, style, option);
        }

        public bool Foldout(GUIContent content, bool foldout, GUIStyle style, params GUILayoutOption[] option)
        {
            var rect = GUILayoutUtility.GetRect(content, style, option);
            return EditorGUI.Foldout(rect, foldout, content, true, style);
        }

        public bool CustomFoldout(string label, bool value, string expandSymbol, string foldSymbol, GUIStyle style, params GUILayoutOption[] option)
        {
            this.Label((value ? foldSymbol : expandSymbol) + label, GUIHelper.FoldoutStyle, option);
            if (UnityEngine.GUI.Button(this.GetLastRect(), GUIContent.none, GUIStyle.none))
                value = !value;
            return value;
        }

        public bool CustomFoldout(string expandSymbol, string foldSymbol, bool value)
        {
            return this.CustomFoldout("", value, expandSymbol, foldSymbol, null, GUILayout.Width(GUIConstants.kFoldoutWidth));
        }

        public bool CustomFoldout(string label, bool value, GUIStyle style, params GUILayoutOption[] option)
        {
            return this.CustomFoldout(label, value, GUIHelper.Folds.DefaultExpandSymbol, GUIHelper.Folds.DefaultFoldSymbol, style, option);
        }

        public bool CustomFoldout(string label, bool value, params GUILayoutOption[] option)
        {
            return this.CustomFoldout(label, value, GUIStyle.none, option);
        }

        public bool CustomFoldout(bool value, params GUILayoutOption[] option)
        {
            return this.CustomFoldout("", value, option);
        }

        public bool CustomFoldout(bool value)
        {
            return this.CustomFoldout("", value, GUILayout.Width(GUIConstants.kFoldoutWidth));
        }
    }
}