using System;

using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;

namespace Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Others
{
    /// <summary>
    /// Similar to Unity's MultilineAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ParagraphAttribute : DrawnAttribute
    {
    }
}