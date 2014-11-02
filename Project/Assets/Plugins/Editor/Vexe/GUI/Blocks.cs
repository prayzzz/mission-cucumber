using UnityEditor;

using UnityEngine;

namespace Assets.Plugins.Editor.Vexe.GUI
{
    public partial class GLWrapper
    {
        private static int indentLevel;
        private int previousLevel;
        private bool guiState;
        private Color guiColor;

        public static int CurrentIndentLevel
        {
            get { return indentLevel; }
            set { indentLevel = value; }
        }

        public static int NextIndentLevel
        {
            get { return CurrentIndentLevel + 1; }
        }

        public void _beginWidth(float width)
        {
            EditorGUIUtility.labelWidth = width;
        }
        public void _endWidth()
        {
            EditorGUIUtility.labelWidth = 0;
        }
        public void _beginV(GUIStyle style)
        {
            EditorGUILayout.BeginVertical(style);
        }
        public void _beginH(GUIStyle style)
        {
            EditorGUILayout.BeginHorizontal(style);
        }
        public void _endV()
        {
            EditorGUILayout.EndVertical();
        }
        public void _endH()
        {
            EditorGUILayout.EndHorizontal();
        }
        public void _beginState(bool newState)
        {
            this.guiState = UnityEngine.GUI.enabled;
            UnityEngine.GUI.enabled = newState;
        }
        public void _endState()
        {
            UnityEngine.GUI.enabled = this.guiState;
        }
        public void _beginColor(Color? color)
        {
            this._beginColor(color.HasValue ? color.Value : Color.white);
        }
        public void _beginColor(Color color)
        {
            this.guiColor = UnityEngine.GUI.color;
            UnityEngine.GUI.color = color;
        }
        public void _endColor()
        {
            UnityEngine.GUI.color = this.guiColor;    
        }
        public void _beginV()
        {
            this._beginV(GUIStyle.none);
        }
        public void _beginH()
        {
            this._beginH(GUIStyle.none);
        }
        public void _beginIndent()
        {
            this._beginIndent(GUIStyle.none);
        }
        public void _beginIndent(GUIStyle style)
        {
            this._beginIndent(style, NextIndentLevel);
        }
        public void _beginIndent(GUIStyle style, int indentLevel)
        {
            this._beginH();
            this.previousLevel = CurrentIndentLevel;
            this.Space(indentLevel * GUIConstants.kIndentAmount);
            this._beginV(style);
        }
        public void _endIndent()
        {
            this._endV();
            CurrentIndentLevel = this.previousLevel;
            this._endH();
        }
        public void _beginChange()
        {
            EditorGUI.BeginChangeCheck();
        }
        public bool _endChange()
        {
            return EditorGUI.EndChangeCheck();
        }
    }
}