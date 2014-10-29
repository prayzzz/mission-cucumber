using System;
using Vexe.Runtime.Types;

namespace Vexe.Editor.Framework.Drawers
{
	public class EnumMaskAttributeDrawer : AttributeDrawer<Enum, EnumMaskAttribute>
	{
		public override void OnGUI()
		{
			var currentValue = dmValue;
			var newMask = gui.EnumMaskFieldThatWorks(currentValue, niceName);
			{
				var newValue = Enum.ToObject(memberType, newMask) as Enum;
				if (!Equals(newValue, currentValue))
				{
					dmValue = newValue;
				}
			}
		}
	}
}