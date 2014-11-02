using System;

using Assets.Plugins.Vexe.Runtime.Serialization;
using Assets.Plugins.Vexe.Runtime.Serialization.Serializers;
using Assets.Plugins.Vexe.Runtime.Serialization.Serializers.FullSerializer;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Categories;

using UnityEngine;

using Vexe.Runtime.Exceptions;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Assets.Plugins.Vexe.Runtime.Types.Core
{
    [DefineCategory("Fields", 0f, MemberType = MemberType.Field, Exclusive = false)]
    [DefineCategory("Properties", 1f, MemberType = MemberType.Property, Exclusive = false)]
    [DefineCategory("Methods", 2f, MemberType = MemberType.Method, Exclusive = false)]
    public abstract class BetterScriptableObject : ScriptableObject, ISavable, IHasUniqueId, ISerializationCallbackReceiver
    {
        public static Type DefaultSerializerType = typeof(fsFullSerializer);

        [SerializeField, DontSave]
        private SerializationData serializationData;
        private SerializationData SerializationData
        {
            get { return this.serializationData ?? (this.serializationData = new SerializationData()); }
        }

        private BaseSerializer serializer;
        public BaseSerializer Serializer
        {
            get { return this.serializer ?? (this.serializer = BaseSerializer.Create(this.SerializerType)); }
        }

        [SerializeField, DontSave]
        private SerializedType serializerType = new SerializedType();
        public Type SerializerType
        {
            get { return this.serializerType.Value ?? (this.serializerType.Value = DefaultSerializerType); }
            set
            {
                if (!value.IsA<BaseSerializer>())
                    throw new TypeMismatchException("Serializer must be a BetterSerializer derivative");

                if (this.serializer == null || this.SerializerType != value)
                {
                    this.serializerType.Value = value;
                    this.serializer = BaseSerializer.Create(value);
                }
            }
        }

        [SerializeField, HideInInspector, DontSave]
        private string id;
        public string ID
        {
            get
            {
                if (this.id.IsNullOrEmpty())
                    this.id = Guid.NewGuid().ToString();
                return this.id;
            }
        }

        public void OnBeforeSerialize()
        {
//#if UNITY_EDITOR
//            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode && !UnityEditor.EditorApplication.isPlaying)
//            {
//                Save();
//            }
//#else
            this.Save();
//#endif
        }

        public void OnAfterDeserialize()
        {
            this.Load();
        }

        public void Save()
        {
            this.dbgLog("Saving " + this.GetType().Name);
            this.SerializationData.Clear();
            this.Serializer.Save(this, this.SerializationData);
        }

        public void Load()
        {
            this.dbgLog("Loading " + this.GetType().Name);
            this.Serializer.Load(this, this.SerializationData);
        }

        public bool dbg;
        protected void dbgLog(string msg, params object[] args)
        {
            if (this.dbg) Log(msg, args);
        }

        protected void dbgLog(object obj)
        {
            if (this.dbg) Log(obj);
        }

        protected static void Log(string msg, params object[] args)
        {
            Debug.Log(string.Format(msg, args));
        }

        protected static void Log(object obj)
        {
            Debug.Log(obj);
        }
    }
}