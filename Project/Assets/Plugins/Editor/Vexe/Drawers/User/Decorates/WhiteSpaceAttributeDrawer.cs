using Assets.Plugins.Editor.Vexe.Drawers.API.Base;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Decorates;

namespace Assets.Plugins.Editor.Vexe.Drawers.User.Decorates
{
    public class WhiteSpaceAttributeDrawer : CompositeDrawer<object, WhitespaceAttribute>
    {
        public override void OnUpperGUI()
        {
            this.gui.Space(this.attribute.Top);
        }
        public override void OnLowerGUI()
        {
            this.gui.Space(this.attribute.Bottom);
        }
        public override void OnRightGUI()
        {
            this.gui.Space(this.attribute.Right);
        }
        public override void OnLeftGUI()
        {
            this.gui.Space(this.attribute.Left);
        }
    }
}