using System;

namespace Vexe.Runtime.Types
{
	/// <summary>
	/// Annotate any member with this attribute to expose it even if it wasn't serializable
	/// This is the only way to expose method and properties with side effects (i.e. not auto-properties)
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Field)]
	public class ShowAttribute : Attribute
	{
	}
}