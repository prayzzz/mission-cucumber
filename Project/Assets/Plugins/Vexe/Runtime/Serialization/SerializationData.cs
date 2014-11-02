using System;
using System.Collections.Generic;

using Vexe.Runtime.Types;

using Object = UnityEngine.Object;

namespace Assets.Plugins.Vexe.Runtime.Serialization
{
    [Serializable]
    public class SerializationData
    {
        public List<Object> serializedObjects = new List<Object>();
        public StrStrDict serializedStrings        = new StrStrDict();

        public void Clear()
        {
            this.serializedObjects.Clear();
            this.serializedStrings.Clear();
        }
    }

    [Serializable]
    public class StrStrDict : KVPList<string, string>
    {
    }
}