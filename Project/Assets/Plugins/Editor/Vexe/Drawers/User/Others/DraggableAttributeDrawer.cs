using UnityEngine;
using Vexe.Editor.Helpers;
using Vexe.Runtime.Types;
using UnityObject = UnityEngine.Object;

namespace Vexe.Editor.Framework.Drawers
{
	public class DraggableAttributeDrawer : CompositeDrawer<UnityObject, DraggableAttribute>
	{
		public override void OnMemberDrawn(Rect rect)
		{
			GUIHelper.RegisterFieldForDrag(rect, dmValue);
		}
	}
}