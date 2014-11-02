using System;

using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;

namespace Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Enums
{
    /// <summary>
    /// Markup an enum with this attribute to take into consideration any custom values set for the enums for correct bit-wise operation
    /// Credits to Bunny82: http://answers.unity3d.com/questions/393992/custom-inspector-multi-select-enum-dropdown.html?sort=oldest
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class EnumMaskAttribute : DrawnAttribute
    {
    }
}