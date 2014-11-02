using System;
using System.Linq;

using Assets.Plugins.Editor.Vexe.Drawers.API.Base;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Others;

using Vexe.Runtime.Extensions;
using Vexe.Runtime.Helpers;
using Vexe.Runtime.Types;

namespace Assets.Plugins.Editor.Vexe.Drawers.User.Others
{
    public class TypeDrawer : AttributeDrawer<Type, ShowTypeAttribute>
    {
        private Type[] availableTypes;
        private string[] typesNames;
        private int index;

        protected override void OnInitialized()
        {
            this.availableTypes = ReflectionHelper.GetAllTypesOf(this.attribute.baseType)
                                                     .Where(t => !t.IsAbstract)
                                                     .ToArray();

            this.typesNames = this.availableTypes.Select(t => t.Name)
                                             .ToArray();
        }

        public override void OnGUI()
        {
            this.index = this.dataMember.IsNull() ? -1 : this.availableTypes.IndexOf(this.dmValue);
            var x = this.gui.Popup(this.niceName, this.index, this.typesNames);
            {
                if (this.index != x)
                {
                    this.dmValue = this.availableTypes[x];
                    this.index = x;
                }
            }
        }
    }
}