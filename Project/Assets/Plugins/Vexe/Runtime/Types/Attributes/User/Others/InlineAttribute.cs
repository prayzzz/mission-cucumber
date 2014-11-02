using System;

using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;

namespace Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Others
{
    /// <summary>
    /// Annotate a Component or GameObject reference with this to draw it in an inline fashion
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class InlineAttribute : CompositeAttribute
    {
        /// <summary>
        /// Whether or not to hide the inlined Component/GameObject
        /// Default: False
        /// </summary>
        public bool HideTarget { get; set; }

        /// <summary>
        /// Inline inside a Gui box?
        /// Default: True
        /// </summary>
        public bool GuiBox { get; set; }

        public InlineAttribute() : this(-1)
        {
        }

        public InlineAttribute(int id) : base(id)
        {
            this.GuiBox = true;
        }
    }
}