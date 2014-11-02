using System;

namespace Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Constraints
{
    /// <summary>
    /// Annotate a string with this attribute to clamp its length between a min and max values
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class StringClampAttribute : RegexAttribute
    {
        public readonly int min;
        public readonly int max;

        public StringClampAttribute(int min, int max)
            : this(-1, min, max)
        {
        }

        public StringClampAttribute(int id, int min, int max)
            : base(id, "^.{" + min + "," + max + "}$")
        {
            this.min = min;
            this.max = max;
        }
    }
}