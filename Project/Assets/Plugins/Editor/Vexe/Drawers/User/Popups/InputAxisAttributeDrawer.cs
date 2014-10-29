using System.Collections.Generic;
using Vexe.Editor.Helpers;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Vexe.Editor.Framework.Drawers
{
	public class InputAxisAttributeDrawer : AttributeDrawer<string, InputAxisAttribute>
	{
		private List<string> axes;
		private int current;

		protected override void OnInitialized()
		{
			if (dmValue == null)
				dmValue = "";

			axes = EditorHelper.GetInputAxes();
			current = axes.IndexOfZeroIfNotFound(dmValue);
		}

		public override void OnGUI()
		{
			var x = gui.Popup(niceName, current, axes.ToArray());
			{
				var newValue = axes[x];
				if (current != x || dmValue != newValue)
				{
					dmValue = newValue;
					current = x;
				}
			}
		}
	}
}