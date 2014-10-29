using System;
using Vexe.Runtime.Serialization;

namespace Vexe.Runtime.Types
{
	public interface ISavable
	{
		void Save();
		void Load();
		Type SerializerType { get; set; }
	}
}