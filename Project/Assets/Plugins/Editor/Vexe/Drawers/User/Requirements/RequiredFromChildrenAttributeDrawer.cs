using System;
using UnityEngine;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Vexe.Editor.Framework.Drawers
{
	public class RequiredFromChildrenAttributeDrawer : RequiredFromRelativeAttributeDrawer<RequiredFromChildrenAttribute>
	{
		protected override Component GetComponentInRelative(GameObject from, Type componentType)
		{
			return from.GetComponentInChildren(componentType);
		}

		protected override GameObject GetRelativeAtPath(GameObject from, string path, bool throwIfNotFound)
		{
			return from.GetChildAtPath(path, throwIfNotFound);
		}

		protected override GameObject GetOrAddRelativeAtPath(GameObject from, string path)
		{
			return from.GetOrAddChildAtPath(path);
		}

		protected override string relative
		{
			get { return "child"; }
		}

		protected override string plural
		{
			get { return "children"; }
		}
	}
}