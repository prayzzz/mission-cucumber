using System;

namespace Assets.Plugins.Vexe.Runtime.Types.Core
{
    public interface ISavable
    {
        void Save();
        void Load();
        Type SerializerType { get; set; }
    }
}