using Assets.Plugins.Editor.Vexe.Drawers.API.Base;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Popups;

using Vexe.Editor.Helpers;
using Vexe.Runtime.Extensions;

namespace Assets.Plugins.Editor.Vexe.Drawers.User.Popups
{
    public class TagsAttributeDrawer : AttributeDrawer<string, TagsAttribute>
    {
        private int current;
        private string[] tags;

        protected override void OnInitialized()
        {
            this.tags = EditorHelper.Tags;
        }

        public override void OnGUI()
        {
            if (this.dmValue != this.tags[this.current])
            {
                this.current = this.tags.IndexOfZeroIfNotFound(this.dmValue);
            }

            var selection = this.gui.Popup(this.niceName, this.current, this.tags);
            {
                if (this.current != selection)
                {
                    this.dmValue = this.tags[selection];
                    this.current = selection;
                }
            }
        }
    }
}