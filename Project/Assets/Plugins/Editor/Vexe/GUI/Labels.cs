using UnityEditor;

using UnityEngine;

namespace Assets.Plugins.Editor.Vexe.GUI
{
    public partial class GLWrapper
    {
        /// <summary>
        /// Creates a bold numeric label with a width of NUMERIC_LABEL_WIDTH
        /// </summary>
        public void NumericLabel(int n, GUIStyle style)
        {
            this.Label(n + "- ", style, GUILayout.Width(GUIConstants.kNumericLabelWidth));
        }

        public void NumericLabel(int n)
        {
            this.NumericLabel(n, EditorStyles.miniLabel);
        }

        /// <summary>
        /// Creates a NumericLabel, with a TextFieldLabel beside it
        /// </summary>
        public void NumericTextFieldLabel(int n, string label)
        {
            this._beginH();
            {
                this.NumericLabel(n);
                this.TextFieldLabel(label);
            }
            this._endH();
        }

        /// <summary>
        /// Creates a Label with a EditorStyles.textField style
        /// </summary>
        public void TextFieldLabel(string label)
        {
            this.Label(label, UnityEngine.GUI.skin.textField);
        }

        public void BoldLabel(string text)
        {
            this.BoldLabel(text, null);
        }

        public void BoldLabel(string text, params GUILayoutOption[] option)
        {
            this.Label(text, EditorStyles.boldLabel, option);
        }

        public void Label(string text)
        {
            this.Label(text, (GUILayoutOption[])null);
        }
        public void Label(string text, params GUILayoutOption[] option)
        {
            this.Label(text, EditorStyles.label, option);
        }
        public void Label(string text, GUIStyle style)
        {
            this.Label(text, style, null);
        }
        public void Label(string text, GUIStyle style, params GUILayoutOption[] option)
        {
            this.Label(GetContent(text), style, option);
        }

        public void Label(GUIContent content, GUIStyle style, params GUILayoutOption[] option)
        {
            GUILayout.Label(content, style, option);
        }

        public void PrefixLabel(GUIContent label)
        {
            this.PrefixLabel(label, GUIStyle.none);
        }
        public void PrefixLabel(string label)
        {
            this.PrefixLabel(label, GUIStyle.none);
        }
        public void PrefixLabel(GUIContent label, GUIStyle labelStyle)
        {
            this.PrefixLabel(label, GUIStyle.none, labelStyle);
        }
        public void PrefixLabel(string label, GUIStyle labelStyle)
        {
            this.PrefixLabel(label, GUIStyle.none, labelStyle);
        }
        public void PrefixLabel(string label, GUIStyle followingStyle, GUIStyle labelStyle)
        {
            this.PrefixLabel(GetContent(label), followingStyle, labelStyle);
        }
        public void PrefixLabel(GUIContent label, GUIStyle followingStyle, GUIStyle labelStyle)
        {
            EditorGUILayout.PrefixLabel(label, followingStyle, labelStyle);
        }
    }
}