﻿using System;

using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;

namespace Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Others
{
    /// <summary>
    /// Similar to Unity's RangeAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class IntSliderAttribute : DrawnAttribute
    {
        public readonly int left, right;

        public IntSliderAttribute(int left, int right)
        {
            this.left  = left;
            this.right = right;
        }
    }

    /// <summary>
    /// Similar to Unity's RangeAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class FloatSliderAttribute : DrawnAttribute
    {
        public readonly float left, right;

        public FloatSliderAttribute(float left, float right)
        {
            this.left  = left;
            this.right = right;
        }
    }
}