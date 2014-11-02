using Assets.Plugins.Editor.Vexe.Drawers.API.Base;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Others;

using UnityEngine;

using Vexe.Runtime.Extensions;

namespace Assets.Plugins.Editor.Vexe.Drawers.User.Others
{
    public class ReadonlyAttributeDrawer<T> : CompositeDrawer<T, ReadonlyAttribute>
    {
        private T previous;

        protected override void OnInitialized()
        {
            this.previous = this.dmValue;
        }

        public override void OnLowerGUI()
        {
            if (!Application.isPlaying && this.attribute.AssignAtEditTime)
                return;

            if (!this.dmValue.GenericEqual(this.previous))
                this.dmValue = this.previous;
        }
    }
}