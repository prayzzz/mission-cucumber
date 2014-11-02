using System;

using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;

using UnityEngine;

namespace Assets.Plugins.Vexe.Runtime.Serialization
{
    public class SerializationAttributes
    {
        public Type[] SerializeMember { get; set; }

        public Type[] DontSerializeMember { get; set; }

        public Type[] SerializableType { get; set; }

        public Type[] Exposure { get; set; }

        public Type[] Hide { get; set; }

        private static SerializationAttributes defaultAttributes;
        public static SerializationAttributes Default
        {
            get
            {
                return defaultAttributes ?? (defaultAttributes = new SerializationAttributes
                {
                    SerializeMember = new Type[]
                    {
                        typeof(SerializeField),
                        typeof(SerializeAttribute),
                        typeof(SaveAttribute)
                    },

                    DontSerializeMember = new Type[]
                    {
                        typeof(NonSerializedAttribute),
                        typeof(DontSerializeAttribute),
                        typeof(DontSaveAttribute),
                    },

                    SerializableType = new Type[]
                    {
                        typeof(SerializableAttribute),
                    },

                    Exposure = new Type[]
                    {
                        typeof(ShowAttribute),
                    },

                    Hide = new Type[]
                    {
                        typeof(HideInInspector),
                        typeof(HideAttribute)
                    },
                });
            }
        }
    }
}