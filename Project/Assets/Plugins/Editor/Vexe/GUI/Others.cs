using UnityEditor;

using UnityEngine;

namespace Assets.Plugins.Editor.Vexe.GUI
{
    public partial class GLWrapper
    {
        public void Splitter(float thickness)
        {
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(thickness));
        }

        public void Splitter()
        {
            this.Splitter(1f);
        }

        public Rect GetLastRect()
        {
            return GUILayoutUtility.GetLastRect();
        }

        public void DrawDefaultInspector(Object target)
        {
            this.DrawDefaultInspector(target, true);
        }

        public void DrawDefaultInspector(Object target, bool excludeScript)
        {
            this.DrawDefaultInspector(new SerializedObject(target), excludeScript);
        }

        public void DrawDefaultInspector(SerializedObject obj, bool excludeScript)
        {
            obj.Update();
            SerializedProperty iterator = obj.GetIterator();
            bool enterChildren = true;
            while (iterator.NextVisible(enterChildren))
            {
                if (excludeScript && iterator.name == "m_Script")
                    continue;
                this.PropertyField(iterator, true);
                enterChildren = false;
            }
            obj.ApplyModifiedProperties();
        }

    }
}