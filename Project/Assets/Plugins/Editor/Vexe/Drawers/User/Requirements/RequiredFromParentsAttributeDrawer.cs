using System;

using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Requirements;

using UnityEngine;

using Vexe.Runtime.Extensions;

namespace Assets.Plugins.Editor.Vexe.Drawers.User.Requirements
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