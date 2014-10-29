using UnityEditor;
using Vexe.Runtime.Types;

namespace Vexe.Editor.Framework.Drawers
{
	public class ParagraphAttributeDrawer : AttributeDrawer<string, ParagraphAttribute>
	{
		protected override void OnInitialized()
		{
			if (dmValue == null)
				dmValue = string.Empty;
		}

		public override void OnGUI()
		{
			gui.Label(niceName);
			dmValue = EditorGUILayout.TextArea(dmValue);
		}
	}
}