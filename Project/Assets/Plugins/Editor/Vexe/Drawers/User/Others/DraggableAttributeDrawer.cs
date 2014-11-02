using Assets.Plugins.Editor.Vexe.Drawers.API.Base;
using Assets.Plugins.Editor.Vexe.Other;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Others;

using UnityEngine;

namespace Assets.Plugins.Editor.Vexe.Drawers.User.Others
{
    public class DraggableAttributeDrawer : CompositeDrawer<Object, DraggableAttribute>
    {
        public override void OnMemberDrawn(Rect rect)
        {
            GUIHelper.RegisterFieldForDrag(rect, this.dmValue);
        }
    }
}