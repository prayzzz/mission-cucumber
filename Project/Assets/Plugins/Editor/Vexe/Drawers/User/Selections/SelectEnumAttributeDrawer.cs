using System;
using Vexe.Editor.Helpers;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Vexe.Editor.Framework.Drawers
{
	public class SelectEnumAttributeDrawer : CompositeDrawer<Enum, SelectEnumAttribute>
	{
		public override void OnRightGUI()
		{
			if (gui.SelectionButton())
			{
				string[] names = Enum.GetNames(memberType);
				int currentIndex = dmValue == null ? -1 : names.IndexOf(dmValue.ToString());
				SelectionWindow.Show(new Tab<string>(
					@getValues: () => names,
					@getCurrent: () => dmValue.ToString(),
					@setTarget: name =>
					{
						if (names[currentIndex] != name)
							dmValue = name.ParseEnum(memberType);
					},
					@getValueName: name => name,
					@title: memberTypeName + "s"
				));
			}
		}
	}
}