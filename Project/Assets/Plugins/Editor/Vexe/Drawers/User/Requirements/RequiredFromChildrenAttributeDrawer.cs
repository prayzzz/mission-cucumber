using System;

using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Requirements;

using UnityEngine;

using Vexe.Runtime.Extensions;

namespace Assets.Plugins.Editor.Vexe.Drawers.User.Requirements
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