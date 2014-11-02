using System;

using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;

namespace Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Filters
{
    /// <summary>
    /// Similar to FilterEnum - but works alongside TagsAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class FilterTagsAttribute : CompositeAttribute
    {
        public FilterTagsAttribute() : this(-1)
        {
        }

        public FilterTagsAttribute(int id) : base(id)
        {
        }
    }
}