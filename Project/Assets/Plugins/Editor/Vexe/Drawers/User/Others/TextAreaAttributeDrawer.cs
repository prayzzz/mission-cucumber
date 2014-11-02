using Assets.Plugins.Editor.Vexe.Drawers.API.Base;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Others;

using UnityEditor;

namespace Assets.Plugins.Editor.Vexe.Drawers.User.Others
{
    public class ParagraphAttributeDrawer : AttributeDrawer<string, ParagraphAttribute>
    {
        protected override void OnInitialized()
        {
            if (this.dmValue == null)
                this.dmValue = string.Empty;
        }

        public override void OnGUI()
        {
            this.gui.Label(this.niceName);
            this.dmValue = EditorGUILayout.TextArea(this.dmValue);
        }
    }
}