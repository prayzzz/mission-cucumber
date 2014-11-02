﻿using System;

namespace Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Constraints
{
    /// <summary>
    /// Annotate an int or float with this attribute to constrain it to a max value
    /// so it won't go above that value
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class MaxAttribute : ConstrainValueAttribute
    {
        public readonly float floatMax;
        public readonly int intMax;

        public MaxAttribute(int id, int max) : base(id)
        {
            this.intMax = max;
        }

        public MaxAttribute(int id, float max) : base(id)
        {
            this.floatMax = max;
        }

        public MaxAttribute(int max) : this(-1, max)
        {
        }

        public MaxAttribute(float max) : this(-1, max)
        {
        }
    }
}