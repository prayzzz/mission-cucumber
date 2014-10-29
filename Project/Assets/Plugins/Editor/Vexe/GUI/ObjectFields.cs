using System;
using UnityEditor;
using UnityEngine;
using Vexe.Editor.Helpers;
using UnityObject = UnityEngine.Object;

namespace Vexe.Editor.Framework.GUIs
{
	public partial class GLWrapper
	{
		public UnityObject DraggableObjectField<T>(T value) where T : UnityObject
		{
			return DraggableObjectField("", value);
		}
		public UnityObject DraggableObjectField<T>(string label, T value) where T : UnityObject
		{
			return DraggableObjectField(label, "", value);
		}
		public UnityObject DraggableObjectField<T>(string label, string tooltip, T value) where T : UnityEngine.Object
		{
			value = ObjectField(label, tooltip, value, true) as T;
			GUIHelper.RegisterFieldForDrag(GetLastRect(), value);
			return value;
		}
		public T ObjectField<T>(T value) where T : UnityObject
		{
			return ObjectField("", value);
		}
		public T ObjectField<T>(string label, T value) where T : UnityObject
		{
			return ObjectField(label, "", value);
		}
		public T ObjectField<T>(string label, string tooltip, T value) where T : UnityObject
		{
			return ObjectField(label, tooltip, value, true);
		}
		public T ObjectField<T>(string label, string tooltip, T value, bool allowSceneObjects) where T : UnityEngine.Object // for some alien reason, writing only "Object" yielded an error that Object is not found even though it's defined and used everywhere in this file
		{
			return ObjectField(GetContent(label, tooltip), value, typeof(T), allowSceneObjects, null) as T;
		}

		public UnityObject ObjectField(UnityObject value)
		{
			return ObjectField("", value);
		}
		public UnityObject ObjectField(UnityObject value, Type type)
		{
			return ObjectField("", value, type);
		}
		public UnityObject ObjectField(string label, UnityObject value)
		{
			return ObjectField(label, value, typeof(UnityObject));
		}
		public UnityObject ObjectField(string label, UnityObject value, Type type)
		{
			return ObjectField(label, value, type, null);
		}
		public UnityObject ObjectField(string label, UnityObject value, Type type, params GUILayoutOption[] option)
		{
			return ObjectField(label, value, type, true, option);
		}
		public UnityObject ObjectField(string label, UnityObject value, Type type, bool allowSceneObjects, params GUILayoutOption[] option)
		{
			return ObjectField(label, "", value, type, allowSceneObjects, option);
		}
		public UnityObject ObjectField(string label, string tooltip, UnityObject value, Type type, params GUILayoutOption[] option)
		{
			return ObjectField(GetContent(label, tooltip), value, type, true, option);
		}
		public UnityObject ObjectField(string label, string tooltip, UnityObject value, Type type, bool allowSceneObjects, params GUILayoutOption[] option)
		{
			return ObjectField(GetContent(label, tooltip), value, type, allowSceneObjects, option);
		}
		public UnityObject ObjectField(GUIContent content, UnityObject value, Type type, bool allowSceneObjects, params GUILayoutOption[] option)
		{
			// If we pass an empty content, ObjectField will still reserve space for an empty label ~__~
			return string.IsNullOrEmpty(content.text) ?
				EditorGUILayout.ObjectField(value, type, allowSceneObjects, option) :
				EditorGUILayout.ObjectField(content, value, type, allowSceneObjects, option);
		}
	}
}