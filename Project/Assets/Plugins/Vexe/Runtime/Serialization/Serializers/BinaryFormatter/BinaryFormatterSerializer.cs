using System;
using System.IO;
using System.Runtime.Serialization;

using Vexe.Runtime.Extensions;

namespace Assets.Plugins.Vexe.Runtime.Serialization.Serializers.BinaryFormatter
{
    [DisplayName("BinaryFormatter")]
    public class BinaryFormatterSerializer : BaseSerializer
    {
        private System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer;
        private SurrogateSelector selector;

        protected override void InternalInitialize()
        {
            Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
            this.serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            this.selector = new SurrogateSelector();
            this.serializer.SurrogateSelector = this.selector;
        }

        public override string Serialize(Type type, object value, object context)
        {
            using (var stream = new MemoryStream())
            {
                this.serializer.Context = new StreamingContext(StreamingContextStates.All, context);
                this.serializer.Serialize(stream, value);
                return stream.ToArray().GetString();
            }
        }

        public override object Deserialize(Type type, string serializedState, object context)
        {
            this.serializer.Context = new StreamingContext(StreamingContextStates.All, context);
            using (var stream = new MemoryStream(serializedState.GetBytes()))
                return this.serializer.Deserialize(stream);
        }

        protected override void AddTypeSurrogate(Type type, Type surrogate)
        {
            //UnityEngine.Debug.Log("Adding surrogate " + surrogate.Name + " for " + type.Name);
            this.selector.AddSurrogate(type, new StreamingContext(), (ISerializationSurrogate)Activator.CreateInstance(surrogate));
            //selector.AddSurrogate(type, new StreamingContext(), surrogate.Instance<ISerializationSurrogate>());
        }

        protected override void AddUnityTypeSurrogate(Type type)
        {
            this.AddTypeSurrogate(type, typeof(UnityObjectSurrogate));
        }
    }
}