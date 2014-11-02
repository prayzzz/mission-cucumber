using System;

using Assets.Plugins.Editor.Vexe.Drawers.API.Base;
using Assets.Plugins.Editor.Vexe.Other;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Selections;

using Vexe.Runtime.Extensions;

namespace Assets.Plugins.Editor.Vexe.Drawers.User.Selections
{
    public class SelectEnumAttributeDrawer : CompositeDrawer<Enum, SelectEnumAttribute>
    {
        public override void OnRightGUI()
        {
            if (this.gui.SelectionButton())
            {
                string[] names = Enum.GetNames(this.memberType);
                int currentIndex = this.dmValue == null ? -1 : names.IndexOf(this.dmValue.ToString());
                SelectionWindow.Show(new Tab<string>(
                    getValues: () => names,
                    getCurrent: () => this.dmValue.ToString(),
                    setTarget: name =>
                    {
                        if (names[currentIndex] != name)
                            this.dmValue = name.ParseEnum(this.memberType);
                    },
                    getValueName: name => name,
                    title: this.memberTypeName + "s"
                ));
            }
        }
    }
}