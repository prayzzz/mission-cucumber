using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;

using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Assets.Plugins.Editor.Vexe.Drawers.API.Base
{
    public abstract class CompositeDrawer<TObjectType, TAttribute> : BaseDrawer where TAttribute : CompositeAttribute
    {
        protected TAttribute attribute               { private set; get; }
        protected DataMember<TObjectType> dataMember { private set; get; }

        protected TObjectType dmValue
        {
            get { return this.dataMember.Value; }
            set { this.dataMember.Value = value; }
        }

        protected sealed override void InternalInitialize()
        {
            this.dataMember = DataMember.Create<TObjectType>(this.memberInfo, this.rawTarget, this.RegisterDirty, this.RegisterUndo);
            this.attribute = this.attributes.GetAttribute<TAttribute>();
        }

        public sealed override void OnGUI()
        {
        }
    }
}