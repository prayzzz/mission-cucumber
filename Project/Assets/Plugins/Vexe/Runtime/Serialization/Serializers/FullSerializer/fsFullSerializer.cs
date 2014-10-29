using System;
using System.Collections.Generic;
using FullSerializer;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Serialization.Serializers.FullSerializer.Converters;
using UnityObject = UnityEngine.Object;

namespace Vexe.Runtime.Serialization.Serializers.FullSerializer
{
	[DisplayName("FullSerializer")]
	public class fsFullSerializer : BaseSerializer
	{
		private fsSerializer serializer;

		protected override void InternalInitialize()
		{
			serializer = new fsSerializer();
		}

		protected override void InitializeLogic()
		{
			base.InitializeLogic();

			fsSerializer.Config = new fsConfig(
				Logic.Attributes.SerializeMember,
				Logic.Attributes.DontSerializeMember);
		}

		public override string Serialize(Type type, object graph, object context)
		{
			// 0- set context
			serializer.Context.Set<List<UnityObject>>(context as List<UnityObject>);

			// 1- serialize the data
			fsData data;
			var fail = serializer.TrySerialize(type, graph, out data);
			if (fail.Failed) throw new Exception(fail.FailureReason);

			// 2- emit the data via JSON
			return fsJsonPrinter.CompressedJson(data);
		}

		public override object Deserialize(Type type, string serializedState, object context)
		{
			// 0- parse the JSON data
			fsData data;
			fsFailure fail = fsJsonParser.Parse(serializedState, out data);
			if (fail.Failed) throw new Exception(fail.FailureReason);

			// 1- set context
			serializer.Context.Set<List<UnityObject>>(context as List<UnityObject>);

			// 2- deserialize the data
			object deserialized = null;
			fail = serializer.TryDeserialize(data, type, ref deserialized);
			if (fail.Failed) throw new Exception(fail.FailureReason);

			return deserialized;
		}

		protected override void AddTypeSurrogate(Type type, Type surrogate)
		{
			serializer.AddConverter(surrogate.Instance<fsConverter>());
		}

		protected override void AddUnityTypeSurrogate(Type type)
		{
			serializer.AddConverter(new UnityObjectConverter());
		}
	}
}