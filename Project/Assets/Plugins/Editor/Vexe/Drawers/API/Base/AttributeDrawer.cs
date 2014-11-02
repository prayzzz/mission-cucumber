using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;

using Vexe.Runtime.Extensions;

namespace Assets.Plugins.Editor.Vexe.Drawers.API.Base
{
    public abstract class AttributeDrawer<TObjectType, TAttribute> : ObjectDrawer<TObjectType> where TAttribute : DrawnAttribute
    {
        protected TAttribute attribute { private set; get; }

        protected sealed override void InternalInitialize()
        {
            base.InternalInitialize();
            this.attribute = this.attributes.GetAttribute<TAttribute>();
        }
    }
}