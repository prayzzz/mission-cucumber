using System;
using UnityEngine;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Vexe.Editor.Framework.Drawers
{
	public class RequiredFromParentsAttributeDrawer : RequiredFromRelativeAttributeDrawer<RequiredFromParentsAttribute>
	{
		protected override Component GetComponentInRelative(GameObject from, Type componentType)
		{
			return from.GetComponentInParent(componentType);
		}

		protected override GameObject GetRelativeAtPath(GameObject from, string path, bool throwIfNotFound)
		{
			return from.GetParentAtPath(path, throwIfNotFound);
		}

		protected override GameObject GetOrAddRelativeAtPath(GameObject from, string path)
		{
			return from.GetOrAddParentAtPath(path);
		}

		protected override string relative
		{
			get { return "parent"; }
		}

		protected override string plural
		{
			get { return "parents"; }
		}
	}
}