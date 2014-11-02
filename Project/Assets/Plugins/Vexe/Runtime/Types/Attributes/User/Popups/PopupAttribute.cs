using System;
using System.Linq;

using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;

namespace Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Popups
{
    /// <summary>
    /// Annotate a string with this attribute to have its value selected from a popup
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class PopupAttribute : DrawnAttribute
    {
        /// <summary>
        /// Use this if you want to dynamically generate the popup values instead of having to hardcode them
        /// The method should have no parameters, and a return type of an array of string, float or int
        /// </summary>
        public string PopulateFrom { get; set; }

        /// <summary>
        /// The popup values
        /// </summary>
        public readonly string[] values;

        public PopupAttribute(string populateFrom)
        {
            this.PopulateFrom = populateFrom;
        }

        public PopupAttribute(params string[] strings)
        {
            this.values = strings;
        }

        public PopupAttribute(params int[] ints)
        {
            this.values = ints.Select(i => i.ToString()).ToArray();
        }

        public PopupAttribute(params float[] floats)
        {
            this.values = floats.Select(f => f.ToString()).ToArray();
        }
    }
}