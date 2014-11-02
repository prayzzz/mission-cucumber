using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;

namespace Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Constraints
{
    public abstract class ConstrainValueAttribute : CompositeAttribute
    {
        public ConstrainValueAttribute()
        {
        }

        public ConstrainValueAttribute(int id) : base(id)
        {
        }
    }
}