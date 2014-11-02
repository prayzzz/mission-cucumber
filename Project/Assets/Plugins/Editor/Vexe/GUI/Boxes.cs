using UnityEditor;

using UnityEngine;

namespace Assets.Plugins.Editor.Vexe.GUI
{
    public partial class GLWrapper
    {
        public void Box(string text, params GUILayoutOption[] option)
        {
            this.Box(text, UnityEngine.GUI.skin.box, option);
        }
        public void Box(string text, GUIStyle style, params GUILayoutOption[] option)
        {
            this.Box(GetContent(text), style, option);
        }

        public void Box(GUIContent content, GUIStyle style, params GUILayoutOption[] option)
        {
            GUILayout.Box(content, style, option);
        }

        public void HelpBox(string message)
        {
            this.HelpBox(message, MessageType.Info);
        }

        public void HelpBox(string message, MessageType type)
        {
            EditorGUILayout.HelpBox(message, type);
        }
    }
}
