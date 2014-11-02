using Assets.Plugins.Editor.Vexe.Drawers.API.Base;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Constraints;

using UnityEngine;

namespace Assets.Plugins.Editor.Vexe.Drawers.User.Constraints
{
    public abstract class ConstrainValueAttributeDrawer<TValue, TAttribute> : CompositeDrawer<TValue, TAttribute>
        where TAttribute : ConstrainValueAttribute
    {
        public override void OnLowerGUI()
        {
            this.dmValue = this.Constrain();
        }

        protected abstract TValue Constrain();
    }

    public class IntMinAttributeDrawer : ConstrainValueAttributeDrawer<int, MinAttribute>
    {
        protected override int Constrain()
        {
            return Mathf.Max(this.dmValue, this.attribute.intMin);
        }
    }

    public class FloatMinAttributeDrawer : ConstrainValueAttributeDrawer<float, MinAttribute>
    {
        protected override float Constrain()
        {
            return Mathf.Max(this.dmValue, this.attribute.floatMin);
        }
    }

    public class IntMaxAttributeDrawer : ConstrainValueAttributeDrawer<int, MaxAttribute>
    {
        protected override int Constrain()
        {
            return Mathf.Min(this.dmValue, this.attribute.intMax);
        }
    }

    public class FloatMaxAttributeDrawer : ConstrainValueAttributeDrawer<float, MaxAttribute>
    {
        protected override float Constrain()
        {
            return Mathf.Min(this.dmValue, this.attribute.floatMax);
        }
    }

    public class IntNumericClampAttributeDrawer : ConstrainValueAttributeDrawer<int, NumericClampAttribute>
    {
        protected override int Constrain()
        {
            return Mathf.Clamp(this.dmValue, this.attribute.intMin, this.attribute.intMax);
        }
    }

    public class FloatNumericClampAttributeDrawer : ConstrainValueAttributeDrawer<float, NumericClampAttribute>
    {
        protected override float Constrain()
        {
            return Mathf.Clamp(this.dmValue, this.attribute.floatMin, this.attribute.floatMax);
        }
    }
}