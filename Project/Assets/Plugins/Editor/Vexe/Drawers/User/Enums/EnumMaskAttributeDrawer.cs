using System;

using Assets.Plugins.Editor.Vexe.Drawers.API.Base;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Enums;

namespace Assets.Plugins.Editor.Vexe.Drawers.User.Enums
{
    public class EnumMaskAttributeDrawer : AttributeDrawer<Enum, EnumMaskAttribute>
    {
        public override void OnGUI()
        {
            var currentValue = this.dmValue;
            var newMask = this.gui.EnumMaskFieldThatWorks(currentValue, this.niceName);
            {
                var newValue = Enum.ToObject(this.memberType, newMask) as Enum;
                if (!Equals(newValue, currentValue))
                {
                    this.dmValue = newValue;
                }
            }
        }
    }
}