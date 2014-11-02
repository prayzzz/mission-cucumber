using Assets.Plugins.Editor.Vexe.Drawers.API.Base;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Others;

namespace Assets.Plugins.Editor.Vexe.Drawers.User.Others
{
    public class IntSliderAttributeDrawer : AttributeDrawer<int, IntSliderAttribute>
    {
        public override void OnGUI()
        {
            this.dmValue = this.gui.IntSlider(this.niceName, this.dmValue, this.attribute.left, this.attribute.right);
        }
    }

    public class FloatSliderAttributeDrawer : AttributeDrawer<float, FloatSliderAttribute>
    {
        public override void OnGUI()
        {
            this.dmValue = this.gui.Slider(this.niceName, this.dmValue, this.attribute.left, this.attribute.right);
        }
    }
}