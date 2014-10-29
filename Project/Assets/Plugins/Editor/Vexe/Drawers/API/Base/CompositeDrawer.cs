using UnityEditor;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Vexe.Editor.Framework.Drawers
{
	public abstract class CompositeDrawer<TObjectType, TAttribute> : BaseDrawer where TAttribute : CompositeAttribute
	{
		protected TAttribute attribute               { private set; get; }
		protected DataMember<TObjectType> dataMember { private set; get; }

		protected TObjectType dmValue
		{
			get { return dataMember.Value; }
			set { dataMember.Value = value; }
		}

		protected sealed override void InternalInitialize()
		{
			dataMember = DataMember.Create<TObjectType>(memberInfo, rawTarget, RegisterDirty, RegisterUndo);
			attribute = attributes.GetAttribute<TAttribute>();
		}

		public sealed override void OnGUI()
		{
		}
	}
}