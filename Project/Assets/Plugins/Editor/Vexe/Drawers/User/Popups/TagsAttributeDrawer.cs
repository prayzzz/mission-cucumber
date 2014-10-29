using Vexe.Editor.Helpers;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Vexe.Editor.Framework.Drawers
{
	public class TagsAttributeDrawer : AttributeDrawer<string, TagsAttribute>
	{
		private int current;
		private string[] tags;

		protected override void OnInitialized()
		{
			tags = EditorHelper.Tags;
		}

		public override void OnGUI()
		{
			if (dmValue != tags[current])
			{
				current = tags.IndexOfZeroIfNotFound(dmValue);
			}

			var selection = gui.Popup(niceName, current, tags);
			{
				if (current != selection)
				{
					dmValue = tags[selection];
					current = selection;
				}
			}
		}
	}
}