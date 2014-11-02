using System;

using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;

namespace Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Selections
{
    /// <summary>
    /// Annotate an enum with this attribute to display the options in a selection window instead of a popup
    /// Useful when the enum has a lot of values that it wouldn't look nice to show it in a popup (KeyCode for ex)
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SelectEnumAttribute : CompositeAttribute
    {
        public SelectEnumAttribute() : this(-1)
        {
        }

        public SelectEnumAttribute(int id) : base(id)
        {
        }
    }
}