using Assets.Plugins.Editor.Vexe.Drawers.API.Base;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Decorates;

using UnityEditor;

namespace Assets.Plugins.Editor.Vexe.Drawers.User.Decorates
{
    public class CommentAttributeDrawer : CompositeDrawer<object, CommentAttribute>
    {
        public override void OnUpperGUI()
        {
            this.gui.HelpBox(this.attribute.comment, (MessageType)this.attribute.type);
        }
    }
}