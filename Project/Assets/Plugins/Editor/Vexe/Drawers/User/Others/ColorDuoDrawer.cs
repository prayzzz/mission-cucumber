using Vexe.Runtime.Types.GUI;

namespace Vexe.Editor.Framework.Drawers
{
	public class ColorDuoDrawer : ObjectDrawer<ColorDuo>
	{
		public override void OnGUI()
		{
			foldout = gui.Foldout(niceName, foldout);

			if (!foldout) return;

			gui._beginV();
			{
				dmValue.FirstColor = gui.ColorField("First", dmValue.FirstColor);
				dmValue.SecondColor = gui.ColorField("Second", dmValue.SecondColor);
			}
			gui._endV();
		}
	}
}