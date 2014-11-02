using System;

using Assets.Plugins.Vexe.Runtime.Examples.Scripts.Serialization;

namespace Assets.Plugins.Vexe.Runtime.Serialization.Serializers.Protobuf
{
    public static class ProtobufSerializableTypes
    {
        public static Type[] Types =
            {
                typeof(ITest),
                typeof(ProtobufExample.MyBaseClass),
                typeof(ProtobufExample.IMyInterface),
                typeof(ProtobufExample.MyGenChild<int>),
            };
    }
}