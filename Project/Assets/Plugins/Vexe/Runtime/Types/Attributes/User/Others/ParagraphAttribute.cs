using System;

namespace Vexe.Runtime.Types
{
	/// <summary>
	/// Similar to Unity's MultilineAttribute
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class ParagraphAttribute : DrawnAttribute
	{
	}
}