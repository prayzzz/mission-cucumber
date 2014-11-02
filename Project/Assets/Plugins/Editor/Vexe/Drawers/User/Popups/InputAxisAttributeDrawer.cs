using System.Collections.Generic;

using Assets.Plugins.Editor.Vexe.Drawers.API.Base;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Popups;

using Vexe.Editor.Helpers;
using Vexe.Runtime.Extensions;

namespace Assets.Plugins.Editor.Vexe.Drawers.User.Popups
{
    public class InputAxisAttributeDrawer : AttributeDrawer<string, InputAxisAttribute>
    {
        private List<string> axes;
        private int current;

        protected override void OnInitialized()
        {
            if (this.dmValue == null)
                this.dmValue = "";

            this.axes = EditorHelper.GetInputAxes();
            this.current = this.axes.IndexOfZeroIfNotFound(this.dmValue);
        }

        public override void OnGUI()
        {
            var x = this.gui.Popup(this.niceName, this.current, this.axes.ToArray());
            {
                var newValue = this.axes[x];
                if (this.current != x || this.dmValue != newValue)
                {
                    this.dmValue = newValue;
                    this.current = x;
                }
            }
        }
    }
}