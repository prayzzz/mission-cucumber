using System;
using System.Linq;
using System.Reflection;

using Assets.Plugins.Editor.Vexe.GUI;
using Assets.Plugins.Editor.Vexe.Other;

using UnityEngine;

using Vexe.Runtime.Exceptions;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Helpers;

using Object = UnityEngine.Object;

namespace Assets.Plugins.Editor.Vexe.CustomEditors
{
    /// <summary>
    /// A better editor class that has a GLWrapper that uses the same API that GUIWrapper does to render GUI controls
    /// with a bunch of most-often used properties and methods for convenience
    /// </summary>
    public class BaseEditor<T> : UnityEditor.Editor where T : Object
    {
        protected GLWrapper gui = new GLWrapper();

        /// <summary>
        /// Returns the typed target object (casts 'target' to the type of component the editor's for)
        /// </summary>
        public T typedTarget { get { return this.target as T; } }

        /// <summary>
        /// A reference to the static globally-available BetterPrefs instance
        /// </summary>
        public BetterEditorPrefs prefs { get { return BetterEditorPrefs.Instance; } }

        /// <summary>
        /// A key that's used in foldout values - override to define a more specific key
        /// </summary>
        protected virtual string id { get { return RTHelper.GetTargetID(this.target); } }

        /// <summary>
        /// [G, S]ets the boolean flag whose key is specified by the key property
        /// </summary>
        protected bool foldout
        {
            get { return this.prefs.GetSafeBool(this.id); }
            set { this.prefs.SetBool(this.id, value); }
        }

        /// <summary>
        /// The editor's target type
        /// </summary>
        private Type mTargetType;
        public Type targetType
        {
            get { return this.mTargetType ?? (this.mTargetType = this.target.GetType()); }
        }

        /// <summary>
        /// Performs a safe-cast to the editor's target to the specified generic argument and returns the result
        /// </summary>
        public TTarget GetCustomTypedTarget<TTarget>() where TTarget : T
        {
            return this.target as TTarget;
        }

        private void OnEnable()
        {
            //TargetType = target.GetType();
            //TypedTarget = target as T;
            this.OnAwake();
        }

        /// <summary>
        /// Override this instead of OnEnable for your editor initialization
        /// </summary>
        protected virtual void OnAwake()
        {
        }

        /// <summary>
        /// Returns a FieldInfo object for the specified field name
        /// The field has to be public, or marked up with SerializeField
        /// null is returned if not found
        /// </summary>
        public FieldInfo GetFieldInfo(string name)
        {
            return this.target.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                    .FirstOrDefault(f => f.Name == name && f.IsPublic ||
                                                    f.IsDefined<SerializeField>());
        }

        /// <summary>
        /// Gets the untyped value of the field whose name is specified by the input argument 'name'
        /// Throws a MemberNotFoundException if the field wasn't found by GetFieldInfo
        /// </summary>
        public object GetFieldValue(string name)
        {
            var field = this.AssertMemberFound(name);
            return field.GetValue(this.target);
        }

        /// <summary>
        /// Gets the typed value of the field whose name is specified by the input argument 'name'
        /// Throws a MemberNotFoundException if the field wasn't found by GetFieldInfo
        /// </summary>
        public TValueType GetFieldValue<TValueType>(string name)
        {
            return (TValueType)this.GetFieldValue(name);
        }

        /// <summary>
        /// Sets the field whose name is specified by the input string to the specified object value
        /// Throws a MemberNotFoundException if the field wasn't found by GetFieldInfo
        /// </summary>
        public void SetFieldValue(string name, object value)
        {
            var field = this.AssertMemberFound(name);
            field.SetValue(this.target, value);
        }

        protected static void Log(string msg, params object[] args)
        {
            Debug.Log(string.Format(msg, args));
        }

        protected static void Log(object msg)
        {
            Debug.Log(msg);
        }

        /// <summary>
        /// A convenience block for Update-Code-Apply on the serializedObject.
        /// Makes it less often to forget about Applying
        /// </summary>
        public void SerializedObjectBlock(Action block)
        {
            this.serializedObject.Update();
            block();
            this.serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Returns a FieldInfo of the specified field if it exists, throws MemberNotFoundException otherwise
        /// </summary>
        private FieldInfo AssertMemberFound(string name)
        {
            var field = this.GetFieldInfo(name);
            if (field == null)
                throw new MemberNotFoundException(name);
            return field;
        }
    }
}