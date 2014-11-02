using System;

using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;

namespace Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Selections
{
    /// <summary>
    /// UnityEngine.Objects annotated with this attribute will have a selection button appear beside them
    /// so that you could select a value from objects in the scene
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SelectObjAttribute : DrawnAttribute
    {
    }
}