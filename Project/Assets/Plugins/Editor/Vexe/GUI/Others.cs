using UnityEngine;
using UnityEditor;
using UnityObject = UnityEngine.Object;

namespace Vexe.Editor.Framework.GUIs
{
	public partial class GLWrapper
	{
		public void Splitter(float thickness)
		{
			GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(thickness));
		}

		public void Splitter()
		{
			Splitter(1f);
		}

		public Rect GetLastRect()
		{
			return GUILayoutUtility.GetLastRect();
		}

		public void DrawDefaultInspector(UnityObject target)
		{
			DrawDefaultInspector(target, true);
		}

		public void DrawDefaultInspector(UnityObject target, bool excludeScript)
		{
			DrawDefaultInspector(new SerializedObject(target), excludeScript);
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
				PropertyField(iterator, true);
				enterChildren = false;
			}
			obj.ApplyModifiedProperties();
		}

	}
}