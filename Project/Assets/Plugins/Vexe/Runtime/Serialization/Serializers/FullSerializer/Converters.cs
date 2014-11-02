using System;
using System.Collections.Generic;

using FullSerializer;

using Object = UnityEngine.Object;

namespace Assets.Plugins.Vexe.Runtime.Serialization.Serializers.FullSerializer
{
    public class UnityObjectConverter : fsConverter
    {
        private List<Object> database
        {
            get { return this.Serializer.Context.Get<List<Object>>(); }
        }

        public override bool CanProcess(Type type)
        {
            return typeof(Object).IsAssignableFrom(type);
        }

        public override fsFailure TrySerialize(object instance, out fsData serialized, Type storageType)
        {
            serialized = new fsData(this.database.Count);
            this.database.Add(instance as Object);
            return fsFailure.Success;
        }

        public override fsFailure TryDeserialize(fsData data, ref object instance, Type storageType)
        {
            instance = this.database[(int)data.AsFloat];
            return fsFailure.Success;
        }

        public override object CreateInstance(fsData data, Type storageType)
        {
            int index = (int)data.AsDictionary["$content"].AsFloat;
            return this.database[index];
        }
    }
}