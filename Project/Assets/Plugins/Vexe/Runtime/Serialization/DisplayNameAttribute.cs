using System;

namespace Vexe.Runtime.Serialization
{
	[AttributeUsage(AttributeTargets.Class)]
	public class DisplayNameAttribute : Attribute
	{
		public readonly string name;

		public DisplayNameAttribute(string name)
		{
			this.name = name;
		}
	}
}