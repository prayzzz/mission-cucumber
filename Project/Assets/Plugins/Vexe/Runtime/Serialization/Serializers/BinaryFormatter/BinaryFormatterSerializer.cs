using System;
using System.IO;
using System.Runtime.Serialization;
using Vexe.Runtime.Extensions;

namespace Vexe.Runtime.Serialization.Serializers.BinaryFormatter
{
	using Surrogates;
	using BinaryFormatter = System.Runtime.Serialization.Formatters.Binary.BinaryFormatter;

	[DisplayName("BinaryFormatter")]
	public class BinaryFormatterSerializer : BaseSerializer
	{
		private BinaryFormatter serializer;
		private SurrogateSelector selector;

		protected override void InternalInitialize()
		{
			Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
			serializer = new BinaryFormatter();
			selector = new SurrogateSelector();
			serializer.SurrogateSelector = selector;
		}

		public override string Serialize(Type type, object value, object context)
		{
			using (var stream = new MemoryStream())
			{
				serializer.Context = new StreamingContext(StreamingContextStates.All, context);
				serializer.Serialize(stream, value);
				return stream.ToArray().GetString();
			}
		}

		public override object Deserialize(Type type, string serializedState, object context)
		{
			serializer.Context = new StreamingContext(StreamingContextStates.All, context);
			using (var stream = new MemoryStream(serializedState.GetBytes()))
				return serializer.Deserialize(stream);
		}

		protected override void AddTypeSurrogate(Type type, Type surrogate)
		{
			//UnityEngine.Debug.Log("Adding surrogate " + surrogate.Name + " for " + type.Name);
			selector.AddSurrogate(type, new StreamingContext(), (ISerializationSurrogate)Activator.CreateInstance(surrogate));
			//selector.AddSurrogate(type, new StreamingContext(), surrogate.Instance<ISerializationSurrogate>());
		}

		protected override void AddUnityTypeSurrogate(Type type)
		{
			AddTypeSurrogate(type, typeof(UnityObjectSurrogate));
		}
	}
}