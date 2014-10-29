using System;
using UnityEditor;
using UnityEngine;
using Vexe.Runtime.Types;

namespace Vexe.Editor.Framework.Drawers
{
	public class RequiredFromThisAttributeDrawer : BaseRequirementAttributeDrawer<RequiredFromThisAttribute>
	{
		protected override Component GetComponent(GameObject from, Type componentType)
		{
			var c = from.GetComponent(componentType);
			if (c == null)
			{
				if (attribute.Add)
					c = from.AddComponent(componentType);
				else
					gui.HelpBox("Couldn't find component in gameObject", MessageType.Warning);
			}
			return c;
		}

		protected override GameObject GetGO(GameObject self)
		{
			return self;
		}
	}
}