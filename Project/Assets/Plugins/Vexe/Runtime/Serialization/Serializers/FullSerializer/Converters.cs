using System;
using System.Collections.Generic;
using FullSerializer;
using UnityObject = UnityEngine.Object;

namespace Vexe.Runtime.Serialization.Serializers.FullSerializer.Converters
{
	public class UnityObjectConverter : fsConverter
	{
		private List<UnityObject> database
		{
			get { return Serializer.Context.Get<List<UnityObject>>(); }
		}

		public override bool CanProcess(Type type)
		{
			return typeof(UnityObject).IsAssignableFrom(type);
		}

		public override fsFailure TrySerialize(object instance, out fsData serialized, Type storageType)
		{
			serialized = new fsData(database.Count);
			database.Add(instance as UnityObject);
			return fsFailure.Success;
		}

		public override fsFailure TryDeserialize(fsData data, ref object instance, Type storageType)
		{
			instance = database[(int)data.AsFloat];
			return fsFailure.Success;
		}

		public override object CreateInstance(fsData data, Type storageType)
		{
			int index = (int)data.AsDictionary["$content"].AsFloat;
			return database[index];
		}
	}
}