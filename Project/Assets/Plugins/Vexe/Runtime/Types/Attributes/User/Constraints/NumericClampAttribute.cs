using System;

namespace Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Constraints
{
    /// <summary>
    /// Annotate an int or float with this attribute to constrain it between a min and max values
    /// i.e. it won't go below min, nor above max
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class NumericClampAttribute : ConstrainValueAttribute
    {
        public readonly int intMin;
        public readonly int intMax;
        public readonly float floatMax;
        public readonly float floatMin;

        public NumericClampAttribute(int id, int min, int max) : base(id)
        {
            this.intMin = min;
            this.intMax = max;
        }

        public NumericClampAttribute(int id, float min, float max) : base(id)
        {
            this.floatMin = min;
            this.floatMax = max;
        }

        public NumericClampAttribute(int min, int max) : this(-1, min, max)
        {
        }

        public NumericClampAttribute(float min, float max) : this(-1, min, max)
        {
        }
    }
}