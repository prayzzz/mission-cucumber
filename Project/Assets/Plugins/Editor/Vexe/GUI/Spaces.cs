using UnityEngine;

namespace Vexe.Editor.Framework.GUIs
{
	public partial class GLWrapper
	{
		public void Indent(float indentLevel)
		{
			Space(indentLevel *GUIConstants.kIndentAmount);
		}

		public void Indent()
		{
			Indent(1f);
		}

		public void Space(float? pixels)
		{
			if (pixels.HasValue)
				Space(pixels.Value);
		}

		public void Space(float pixels)
		{
			GUILayout.Space(pixels);
		}

		public void FlexibleSpace()
		{
			GUILayout.FlexibleSpace();
		}
	}
}
