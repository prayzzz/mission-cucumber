using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using ProtoBuf;
using ProtoBuf.Meta;

using Vexe.Runtime.Extensions;

namespace Assets.Plugins.Vexe.Runtime.Serialization.Serializers.Protobuf
{
    [DisplayName("Protobuf-net")]
    public class ProtobufSerializer : BaseSerializer
    {
        private RuntimeTypeModel serializer;

        void log(string msg, params object[] args)
        {
            UnityEngine.Debug.Log(string.Format(msg, args));
        }

        void log(string msg)
        {
            this.log(msg, new object[0]);
        }

        protected override void InternalInitialize()
        {
            this.serializer = TypeModel.Create();

            var resolver = new ProtobufTypeResolver(this.serializer);
            resolver.ResolveTypes(ProtobufSerializableTypes.Types, type =>
                this.Logic.GetSerializableMembers(type, null).Select(m => m.Name));
        }

        public override string Serialize(Type type, object value, object context)
        {
            using (var stream = new MemoryStream())
            {
                this.serializer.Serialize(stream, value, new SerializationContext { Context = context });
                return stream.ToArray().GetString();
            }
        }

        public override object Deserialize(Type type, string serializedState, object context)
        {
            using (var stream = new MemoryStream(serializedState.GetBytes()))
                return this.serializer.Deserialize(stream, null, type, new SerializationContext { Context = context });
        }

        protected override void AddTypeSurrogate(Type type, Type surrogate)
        {
            this.serializer.Add(type, true).SetSurrogate(surrogate);
        }

        protected override void AddUnityTypeSurrogate(Type type)
        {
            this.AddTypeSurrogate(type, typeof(UnityObjectSurrogate<>).MakeGenericType(type));
        }

        public class ProtobufTypeResolver
        {
            private readonly RuntimeTypeModel model;

            public ProtobufTypeResolver(RuntimeTypeModel model)
            {
                this.model = model;
            }

            public void ResolveTypes(Type[] types, Func<Type, IEnumerable<string>> getMemberNames)
            {
                for (int i = 0; i < types.Length; i++)
                    this.ResolveType(types[i]);

                foreach (MetaType metaType in this.model.GetTypes())
                {
                    var members = getMemberNames(metaType.Type).OrderBy(x => x).ToArray();
                    this.log("adding members {0} to type {1}", string.Join(", ", members), metaType.Type.Name);
                    metaType.Add(members);
                }
            }

            private void ResolveType(Type type)
            {
                var metaType = this.AddType(type);
                var children = type.GetChildren()
                                         .OrderBy(t => t.FullName)
                                         .ToList();

                for (int i = 0; i < children.Count; i++)
                {
                    var child = children[i];
                    this.AddType(child);
                    this.log("adding subtype {0} to base type {1} with id {2}", child.Name, type.Name, i + 1);
                    metaType.AddSubType(i + 1, child);
                }
            }
            
            private MetaType AddType(Type type)
            {
                this.log("adding type: {0}", type.Name);
                return this.model.Add(type, false);
            }

            void log(string msg, params object[] args)
            {
                //UnityEngine.Debug.Log(string.Format(msg, args));
            }

            void log(string msg)
            {
                this.log(msg, new object[0]);
            }
        }
    }
}