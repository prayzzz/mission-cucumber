using System;
using System.Collections.Generic;

using FullSerializer;

using Vexe.Runtime.Extensions;

using Object = UnityEngine.Object;

namespace Assets.Plugins.Vexe.Runtime.Serialization.Serializers.FullSerializer
{
    [DisplayName("FullSerializer")]
    public class fsFullSerializer : BaseSerializer
    {
        private fsSerializer serializer;

        protected override void InternalInitialize()
        {
            this.serializer = new fsSerializer();
        }

        protected override void InitializeLogic()
        {
            base.InitializeLogic();

            fsSerializer.Config = new fsConfig(
                this.Logic.Attributes.SerializeMember,
                this.Logic.Attributes.DontSerializeMember);
        }

        public override string Serialize(Type type, object graph, object context)
        {
            // 0- set context
            this.serializer.Context.Set<List<Object>>(context as List<Object>);

            // 1- serialize the data
            fsData data;
            var fail = this.serializer.TrySerialize(type, graph, out data);
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
            this.serializer.Context.Set<List<Object>>(context as List<Object>);

            // 2- deserialize the data
            object deserialized = null;
            fail = this.serializer.TryDeserialize(data, type, ref deserialized);
            if (fail.Failed) throw new Exception(fail.FailureReason);

            return deserialized;
        }

        protected override void AddTypeSurrogate(Type type, Type surrogate)
        {
            this.serializer.AddConverter(surrogate.Instance<fsConverter>());
        }

        protected override void AddUnityTypeSurrogate(Type type)
        {
            this.serializer.AddConverter(new UnityObjectConverter());
        }
    }
}