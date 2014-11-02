using System;

using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Requirements;

using UnityEditor;

using UnityEngine;

using Vexe.Runtime.Types;

using Object = UnityEngine.Object;

namespace Assets.Plugins.Editor.Vexe.Drawers.User.Requirements
{
    public class RequiredAttributeDrawer : BaseRequirementAttributeDrawer<RequiredAttribute>
    {
        protected override Component GetComponent(GameObject from, Type componentType)
        {
            Component result = null;
            if (this.dataMember.IsNull())
            {
                result = Object.FindObjectOfType(this.memberType) as Component;
                if (result == null)
                {
                    if (this.attribute.Add && !this.memberType.IsAbstract)
                    {
                        var go = new GameObject("[Required] " + this.memberTypeName);
                        result = go.AddComponent(this.memberType);
                    }
                }

                if (result == null)
                    this.gui.HelpBox("Reference is required but is yet to be assigned...", MessageType.Warning);
            }

            return result;
        }

        protected override GameObject GetGO(GameObject from)
        {
            GameObject result = null;
            if (this.dataMember.IsNull())
            {
                if (this.attribute.Add)
                    result = new GameObject("[Required] " + this.dataMember.Name);
                else result = GameObject.Find(this.dataMember.Name);

                if (result == null)
                    this.gui.HelpBox("Reference is required but is yet to be assigned...", MessageType.Warning);
            }

            return result;
        }
    }
}