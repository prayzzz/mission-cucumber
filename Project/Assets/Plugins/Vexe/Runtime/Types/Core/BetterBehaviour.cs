using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Serialization;
using Vexe.Runtime.Serialization.Serializers;
using Vexe.Runtime.Serialization.Serializers.FullSerializer;

namespace Vexe.Runtime.Types
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
			get { return serializationData ?? (serializationData = new SerializationData()); }
		}

		private BaseSerializer serializer;
		public BaseSerializer Serializer
		{
			get { return serializer ?? (serializer = BaseSerializer.Create(SerializerType)); }
		}

		[SerializeField, DontSave]
		private SerializedType serializerType = new SerializedType();
		public Type SerializerType
		{
			get { return serializerType.Value ?? (serializerType.Value = DefaultSerializerType); }
			set
			{
				if (value != null && !string.IsNullOrEmpty(value.Name) && (serializer == null || SerializerType != value))
				{
					serializer = BaseSerializer.Create(value);
					serializerType.Value = value;
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
				if (id.IsNullOrEmpty())
					id = Guid.NewGuid().ToString();
				return id;
			}
		}
		#endregion

		// cached transform - position/rotation/scale [g|s]etters
		#region
		private Transform cachedTransform;
		new public Transform transform
		{
			get { if (cachedTransform == null) cachedTransform = base.transform; return cachedTransform; }
		}

		public Vector3 forward
		{
			get { return transform.forward; }
			set { transform.forward = value; }
		}

		public Vector3 right
		{
			get { return transform.right; }
			set { transform.right = value; }
		}

		public Vector3 left
		{
			get { return -right; }
			set { right = -value; }
		}

		public Vector3 up
		{
			get { return transform.up; }
			set { transform.up = value; }
		}

		public Vector3 back
		{
			get { return -up; }
			set { up = -value; }
		}

		public Vector3 position
		{
			get { return transform.position; }
			set { transform.position = value; }
		}

		public Vector3 localPosition
		{
			get { return transform.localPosition; }
			set { transform.localPosition = value; }
		}

		public Quaternion rotation
		{
			get { return transform.rotation; }
			set { transform.rotation = value; }
		}

		public Quaternion localRotation
		{
			get { return transform.localRotation; }
			set { transform.localRotation = value; }
		}

		public Vector3 eulerAngles
		{
			get { return transform.eulerAngles; }
			set { transform.eulerAngles = value; }
		}

		public Vector3 localEulerAngles
		{
			get { return transform.localEulerAngles; }
			set { transform.localEulerAngles = value; }
		}

		public Vector3 localScale
		{
			get { return transform.localScale; }
			set { transform.localScale = value; }
		}
		#endregion

		// [De]serialization - Save/Load
		#region 
		public void OnBeforeSerialize()
		{
#if UNITY_EDITOR
			if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode && !UnityEditor.EditorApplication.isPlaying)
			{
				Save();
			}
#else
			Save();
#endif
		}

		public void OnAfterDeserialize()
		{
			Load();
		}

		public void Save()
		{
			dbgLog("Saving " + GetType().Name);
			SerializationData.Clear();
			Serializer.Save(this, SerializationData);
		}

		public void Load()
		{
			dbgLog("Loading" + GetType().Name);
			Serializer.Load(this, SerializationData);
		}
		#endregion

		// Logging
		#region
		protected void dbgLog(string msg, params object[] args)
		{
			if (dbg) Log(msg, args);
		}

		protected void dbgLog(object obj)
		{
			if (dbg) Log(obj);
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
			var type = GetType();
			dbgLog("Assigning default values if any to " + type.Name);

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