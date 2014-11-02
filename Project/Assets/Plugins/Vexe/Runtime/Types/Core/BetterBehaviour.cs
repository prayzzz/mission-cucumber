using System;
using System.Linq;
using System.Reflection;

using Assets.Plugins.Vexe.Runtime.Serialization;
using Assets.Plugins.Vexe.Runtime.Serialization.Serializers;
using Assets.Plugins.Vexe.Runtime.Serialization.Serializers.FullSerializer;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Categories;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Others;

using UnityEngine;

using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Assets.Plugins.Vexe.Runtime.Types.Core
{
    /// <summary>
    /// Inherit from this instead of MonoBehaviour to live in a better world!
    /// </summary>
    [DefineCategory("Fields", 0f, MemberType = MemberType.Field, Exclusive = false)]
    [DefineCategory("Properties", 1f, MemberType = MemberType.Property, Exclusive = false)]
    [DefineCategory("Methods", 2f, MemberType = MemberType.Method, Exclusive = false)]
    [DefineCategory("Debug", 3f, Pattern = "^dbg")]
    public abstract class BetterBehaviour : MonoBehaviour, ISavable, IHasUniqueId, ISerializationCallbackReceiver
    {
        /// <summary>
        /// Use this to include members to the "Debug" categories
        /// Ex:
        /// [Category(Dbg)]
        /// public Color gizmosColor;
        /// </summary>
        protected const string Dbg = "Debug";

        /// <summary>
        /// Used for debugging/logging
        /// </summary>
        [DontSave] public bool dbg;

        // Serializer[Type] - SerializationData
        #region
        public static Type DefaultSerializerType = typeof(fsFullSerializer);

        /// <summary>
        /// It's important to let *only* unity serialize our serialization data
        /// </summary>
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
                if (value != null && !string.IsNullOrEmpty(value.Name) && (this.serializer == null || this.SerializerType != value))
                {
                    this.serializer = BaseSerializer.Create(value);
                    this.serializerType.Value = value;
                }
            }
        }
        #endregion

        // IHasUniqueId implementation
        #region
        /// <summary>
        /// A unique identifier used primarly from editor scripts to have editor data persist
        /// I've had some runtime usages for it too
        /// </summary>
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
        #endregion

        // cached transform - position/rotation/scale [g|s]etters
        #region
        private Transform cachedTransform;
        new public Transform transform
        {
            get { if (this.cachedTransform == null) this.cachedTransform = base.transform; return this.cachedTransform; }
        }

        public Vector3 forward
        {
            get { return this.transform.forward; }
            set { this.transform.forward = value; }
        }

        public Vector3 right
        {
            get { return this.transform.right; }
            set { this.transform.right = value; }
        }

        public Vector3 left
        {
            get { return -this.right; }
            set { this.right = -value; }
        }

        public Vector3 up
        {
            get { return this.transform.up; }
            set { this.transform.up = value; }
        }

        public Vector3 back
        {
            get { return -this.up; }
            set { this.up = -value; }
        }

        public Vector3 position
        {
            get { return this.transform.position; }
            set { this.transform.position = value; }
        }

        public Vector3 localPosition
        {
            get { return this.transform.localPosition; }
            set { this.transform.localPosition = value; }
        }

        public Quaternion rotation
        {
            get { return this.transform.rotation; }
            set { this.transform.rotation = value; }
        }

        public Quaternion localRotation
        {
            get { return this.transform.localRotation; }
            set { this.transform.localRotation = value; }
        }

        public Vector3 eulerAngles
        {
            get { return this.transform.eulerAngles; }
            set { this.transform.eulerAngles = value; }
        }

        public Vector3 localEulerAngles
        {
            get { return this.transform.localEulerAngles; }
            set { this.transform.localEulerAngles = value; }
        }

        public Vector3 localScale
        {
            get { return this.transform.localScale; }
            set { this.transform.localScale = value; }
        }
        #endregion

        // [De]serialization - Save/Load
        #region 
        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode && !UnityEditor.EditorApplication.isPlaying)
            {
                this.Save();
            }
#else
            Save();
#endif
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
            this.dbgLog("Loading" + this.GetType().Name);
            this.Serializer.Load(this, this.SerializationData);
        }
        #endregion

        // Logging
        #region
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
            if (args == null) args = new object[0];
            Debug.Log(string.Format(msg, args));
        }

        protected static void Log(object obj)
        {
            Debug.Log(obj);
        }
        #endregion

        public void Reset()
        {
            var type = this.GetType();
            this.dbgLog("Assigning default values if any to " + type.Name);

            var defaults = from member in DataMember.Enumerate(type, this, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                let defaultAttribute = member.Info.GetCustomAttribute<DefaultAttribute>()
                                where defaultAttribute != null
                                select new { Attribute = defaultAttribute, Member = member };

            foreach (var x in defaults)
            {
                x.Member.Set(x.Attribute.Value);
            }
        }
    }
}