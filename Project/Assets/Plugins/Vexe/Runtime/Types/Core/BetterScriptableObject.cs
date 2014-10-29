using System;
using UnityEngine;
using Vexe.Runtime.Exceptions;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Serialization;
using Vexe.Runtime.Serialization.Serializers;
using Vexe.Runtime.Serialization.Serializers.FullSerializer;

namespace Vexe.Runtime.Types
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
				if (!value.IsA<BaseSerializer>())
					throw new TypeMismatchException("Serializer must be a BetterSerializer derivative");

				if (serializer == null || SerializerType != value)
				{
					serializerType.Value = value;
					serializer = BaseSerializer.Create(value);
				}
			}
		}

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

		public void OnBeforeSerialize()
		{
//#if UNITY_EDITOR
//			if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode && !UnityEditor.EditorApplication.isPlaying)
//			{
//				Save();
//			}
//#else
			Save();
//#endif
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
			dbgLog("Loading " + GetType().Name);
			Serializer.Load(this, SerializationData);
		}

		public bool dbg;
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
			Debug.Log(string.Format(msg, args));
		}

		protected static void Log(object obj)
		{
			Debug.Log(obj);
		}
	}
}