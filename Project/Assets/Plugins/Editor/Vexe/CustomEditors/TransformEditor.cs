using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Vexe.Editor.Framework.Drawers;
using Vexe.Editor.Framework.GUIs;
using Vexe.Runtime.Types;
using SP = UnityEditor.SerializedProperty;
using UnityObject = UnityEngine.Object;

namespace Vexe.Editor.Framework.Editors
{
	[CanEditMultipleObjects]
	//[CustomEditor(typeof(Transform))]
	public class TransformEditor : BaseEditor<Transform>
	{
		private static TransformDrawer transformDrawer;

		protected override void OnAwake()
		{
			if (serializedObject != null || !serializedObject.Equals(null))
				Init();
		}

		private void Init()
		{
			transformDrawer = new TransformDrawer(
				serializedObject.FindProperty("m_LocalPosition"),
				serializedObject.FindProperty("m_LocalRotation"),
				serializedObject.FindProperty("m_LocalScale"),
				gui, serializedObject.targetObject);

			//Log("Init transform editor");
		}

		public override void OnInspectorGUI()
		{
			//if (serializedObject.Equals(null))
			//{
				Init();
			//}
			//else
			//{
				SerializedObjectBlock(transformDrawer.OnGUI);
			//}
		}
	}

	public class TransformDrawer
	{
		private BetterVector3Drawer posDrawer;
		private BetterVector3Drawer rotDrawer;
		private BetterVector3Drawer sclDrawer;

		public TransformDrawer(SP spPos, SP spRot, SP spScl, GLWrapper gui, UnityObject target)
		{
			Func<string, string, Func<Vector3>, Action<Vector3>, DataInfo> newElement = (id, name, getter, setter) =>
				new DataInfo(
					@id           : id,
					@getter       : () => getter(),
					@setter       : x => setter((Vector3)x),
					@attributes   : null,
					@name         : name,
					@elementType  : typeof(Vector3),
					@declaringType: target.GetType(),
					@reflectedType: GetType()
				);

			Action<Vector3> setQuat = eulerAngles =>
			{
				if (!eulerAngles.Equals(spRot.quaternionValue.eulerAngles))
				{
					var q = new Quaternion();
					q.eulerAngles = eulerAngles;
					spRot.quaternionValue = q;
				}
			};

			init(newElement("1", "Position", () => spPos.vector3Value, x => spPos.vector3Value = x),
				posDrawer = new BetterVector3Drawer(), target, gui);
			init(newElement("2", "Rotation", () => spRot.quaternionValue.eulerAngles, setQuat),
				rotDrawer = new BetterVector3Drawer(), target, gui);
			init(newElement("3", "Scale", () => spScl.vector3Value, x => spScl.vector3Value = x),
				sclDrawer = new BetterVector3Drawer(), target, gui);
		}

		void init(MemberInfo info, BaseDrawer drawer, UnityObject target, GLWrapper gui)
		{
			drawer.Initialize(info, new Attribute[0], target, target, gui, null);
		}

		public void OnGUI()
		{
			posDrawer.OnGUI();
			rotDrawer.OnGUI();
			sclDrawer.OnGUI();
		}
	}
}