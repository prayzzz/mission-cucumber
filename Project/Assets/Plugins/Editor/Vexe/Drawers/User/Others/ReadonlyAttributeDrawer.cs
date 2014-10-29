using UnityEngine;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Vexe.Editor.Framework.Drawers
{
	public class ReadonlyAttributeDrawer<T> : CompositeDrawer<T, ReadonlyAttribute>
	{
		private T previous;

		protected override void OnInitialized()
		{
			previous = dmValue;
		}

		public override void OnLowerGUI()
		{
			if (!Application.isPlaying && attribute.AssignAtEditTime)
				return;

			if (!dmValue.GenericEqual(previous))
				dmValue = previous;
		}
	}
}