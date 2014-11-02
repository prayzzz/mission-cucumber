using System;
using System.Reflection;

using Assets.Plugins.Editor.Vexe.Drawers.API.Base;
using Assets.Plugins.Editor.Vexe.Drawers.User.Vectors;
using Assets.Plugins.Editor.Vexe.GUI;

using UnityEditor;

using UnityEngine;

using Vexe.Runtime.Types;

using Object = UnityEngine.Object;

namespace Assets.Plugins.Editor.Vexe.CustomEditors
{
    [CanEditMultipleObjects]
    //[CustomEditor(typeof(Transform))]
    public class TransformEditor : BaseEditor<Transform>
    {
        private static TransformDrawer transformDrawer;

        protected override void OnAwake()
        {
            if (this.serializedObject != null || !this.serializedObject.Equals(null))
                this.Init();
        }

        private void Init()
        {
            transformDrawer = new TransformDrawer(
                this.serializedObject.FindProperty("m_LocalPosition"),
                this.serializedObject.FindProperty("m_LocalRotation"),
                this.serializedObject.FindProperty("m_LocalScale"),
                this.gui, this.serializedObject.targetObject);

            //Log("Init transform editor");
        }

        public override void OnInspectorGUI()
        {
            //if (serializedObject.Equals(null))
            //{
                this.Init();
            //}
            //else
            //{
                this.SerializedObjectBlock(transformDrawer.OnGUI);
            //}
        }
    }

    public class TransformDrawer
    {
        private BetterVector3Drawer posDrawer;
        private BetterVector3Drawer rotDrawer;
        private BetterVector3Drawer sclDrawer;

        public TransformDrawer(SerializedProperty spPos, SerializedProperty spRot, SerializedProperty spScl, GLWrapper gui, Object target)
        {
            Func<string, string, Func<Vector3>, Action<Vector3>, DataInfo> newElement = (id, name, getter, setter) =>
                new DataInfo(
                    id           : id,
                    getter       : () => getter(),
                    setter       : x => setter((Vector3)x),
                    attributes   : null,
                    name         : name,
                    elementType  : typeof(Vector3),
                    declaringType: target.GetType(),
                    reflectedType: this.GetType()
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

            this.init(newElement("1", "Position", () => spPos.vector3Value, x => spPos.vector3Value = x),
                this.posDrawer = new BetterVector3Drawer(), target, gui);
            this.init(newElement("2", "Rotation", () => spRot.quaternionValue.eulerAngles, setQuat),
                this.rotDrawer = new BetterVector3Drawer(), target, gui);
            this.init(newElement("3", "Scale", () => spScl.vector3Value, x => spScl.vector3Value = x),
                this.sclDrawer = new BetterVector3Drawer(), target, gui);
        }

        void init(MemberInfo info, BaseDrawer drawer, Object target, GLWrapper gui)
        {
            drawer.Initialize(info, new Attribute[0], target, target, gui, null);
        }

        public void OnGUI()
        {
            this.posDrawer.OnGUI();
            this.rotDrawer.OnGUI();
            this.sclDrawer.OnGUI();
        }
    }
}