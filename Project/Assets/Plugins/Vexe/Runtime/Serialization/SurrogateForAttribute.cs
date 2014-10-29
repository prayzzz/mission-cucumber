using System;

namespace Vexe.Runtime.Serialization
{
	/// <summary>
	/// Apply this attribute to the surrogates you write,
	/// providing what the surrogate is for and for which serializer
	/// </summary>
	public class SurrogateForAttribute : Attribute
	{
		public readonly Type forType;
		public readonly Type forSerializer;

		public SurrogateForAttribute(Type forType, Type forSerializer)
		{
			this.forType       = forType;
			this.forSerializer = forSerializer;
		}
	}
}