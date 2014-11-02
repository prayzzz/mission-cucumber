using System;

using UnityEngine;

namespace Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Categories
{
    /// <summary>
    /// Annotate a member with this to include it in a certain category with a certain display order
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public class CategoryAttribute : PropertyAttribute
    {
        /// <summary>
        /// The name of the category to add the member to
        /// </summary>
        public readonly string name;

        /// <summary>
        /// The display order of the member inside that category
        /// </summary>
        public readonly float displayOrder;

        public CategoryAttribute(string name, float displayOrder = -1)
        {
            this.name = name;
            this.displayOrder = displayOrder;
        }
    }
}