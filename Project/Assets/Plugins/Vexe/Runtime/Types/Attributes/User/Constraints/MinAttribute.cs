using System;

namespace Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Constraints
{
    /// <summary>
    /// Annotate an int or float with this attribute to constrain it to a min value
    /// so it won't go below that value
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class MinAttribute : ConstrainValueAttribute
    {
        public readonly float floatMin;
        public readonly int intMin;

        public MinAttribute(int id, int min) : base(id)
        {
            this.intMin = min;
        }

        public MinAttribute(int id, float min) : base(id)
        {
            this.floatMin = min;
        }

        public MinAttribute(int min) : this(-1, min)
        {
        }

        public MinAttribute(float min) : this(-1, min)
        {
        }
    }
}