using System;

using Assets.Plugins.Editor.Vexe.Other;

using UnityEditor;

using UnityEngine;

using Object = UnityEngine.Object;

namespace Assets.Plugins.Editor.Vexe.GUI
{
    public partial class GLWrapper
    {
        public Object DraggableObjectField<T>(T value) where T : Object
        {
            return this.DraggableObjectField("", value);
        }
        public Object DraggableObjectField<T>(string label, T value) where T : Object
        {
            return this.DraggableObjectField(label, "", value);
        }
        public Object DraggableObjectField<T>(string label, string tooltip, T value) where T : UnityEngine.Object
        {
            value = this.ObjectField(label, tooltip, value, true) as T;
            GUIHelper.RegisterFieldForDrag(this.GetLastRect(), value);
            return value;
        }
        public T ObjectField<T>(T value) where T : Object
        {
            return this.ObjectField("", value);
        }
        public T ObjectField<T>(string label, T value) where T : Object
        {
            return this.ObjectField(label, "", value);
        }
        public T ObjectField<T>(string label, string tooltip, T value) where T : Object
        {
            return this.ObjectField(label, tooltip, value, true);
        }
        public T ObjectField<T>(string label, string tooltip, T value, bool allowSceneObjects) where T : UnityEngine.Object // for some alien reason, writing only "Object" yielded an error that Object is not found even though it's defined and used everywhere in this file
        {
            return this.ObjectField(GetContent(label, tooltip), value, typeof(T), allowSceneObjects, null) as T;
        }

        public Object ObjectField(Object value)
        {
            return this.ObjectField("", value);
        }
        public Object ObjectField(Object value, Type type)
        {
            return this.ObjectField("", value, type);
        }
        public Object ObjectField(string label, Object value)
        {
            return this.ObjectField(label, value, typeof(Object));
        }
        public Object ObjectField(string label, Object value, Type type)
        {
            return this.ObjectField(label, value, type, null);
        }
        public Object ObjectField(string label, Object value, Type type, params GUILayoutOption[] option)
        {
            return this.ObjectField(label, value, type, true, option);
        }
        public Object ObjectField(string label, Object value, Type type, bool allowSceneObjects, params GUILayoutOption[] option)
        {
            return this.ObjectField(label, "", value, type, allowSceneObjects, option);
        }
        public Object ObjectField(string label, string tooltip, Object value, Type type, params GUILayoutOption[] option)
        {
            return this.ObjectField(GetContent(label, tooltip), value, type, true, option);
        }
        public Object ObjectField(string label, string tooltip, Object value, Type type, bool allowSceneObjects, params GUILayoutOption[] option)
        {
            return this.ObjectField(GetContent(label, tooltip), value, type, allowSceneObjects, option);
        }
        public Object ObjectField(GUIContent content, Object value, Type type, bool allowSceneObjects, params GUILayoutOption[] option)
        {
            // If we pass an empty content, ObjectField will still reserve space for an empty label ~__~
            return string.IsNullOrEmpty(content.text) ?
                EditorGUILayout.ObjectField(value, type, allowSceneObjects, option) :
                EditorGUILayout.ObjectField(content, value, type, allowSceneObjects, option);
        }
    }
}