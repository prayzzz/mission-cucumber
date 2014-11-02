using System;

using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Requirements;

using UnityEditor;

using UnityEngine;

namespace Assets.Plugins.Editor.Vexe.Drawers.User.Requirements
{
    public class RequiredFromThisAttributeDrawer : BaseRequirementAttributeDrawer<RequiredFromThisAttribute>
    {
        protected override Component GetComponent(GameObject from, Type componentType)
        {
            var c = from.GetComponent(componentType);
            if (c == null)
            {
                if (this.attribute.Add)
                    c = from.AddComponent(componentType);
                else
                    this.gui.HelpBox("Couldn't find component in gameObject", MessageType.Warning);
            }
            return c;
        }

        protected override GameObject GetGO(GameObject self)
        {
            return self;
        }
    }
}