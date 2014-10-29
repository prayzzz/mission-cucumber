using Vexe.Runtime.Types;

namespace Vexe.Editor.Framework.Drawers
{
	public class IntSliderAttributeDrawer : AttributeDrawer<int, IntSliderAttribute>
	{
		public override void OnGUI()
		{
			dmValue = gui.IntSlider(niceName, dmValue, attribute.left, attribute.right);
		}
	}

	public class FloatSliderAttributeDrawer : AttributeDrawer<float, FloatSliderAttribute>
	{
		public override void OnGUI()
		{
			dmValue = gui.Slider(niceName, dmValue, attribute.left, attribute.right);
		}
	}
}