using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Vexe.Editor.Framework.Drawers
{
	public abstract class AttributeDrawer<TObjectType, TAttribute> : ObjectDrawer<TObjectType> where TAttribute : DrawnAttribute
	{
		protected TAttribute attribute { private set; get; }

		protected sealed override void InternalInitialize()
		{
			base.InternalInitialize();
			attribute = attributes.GetAttribute<TAttribute>();
		}
	}
}