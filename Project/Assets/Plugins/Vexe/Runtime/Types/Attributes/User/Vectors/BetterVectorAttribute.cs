using System;

using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;

namespace Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Vectors
{
    /// <summary>
    /// Annotate vectors (2/3) with this to get a foldout besides the vector offering more functionalities (copy, paste, normalize, randomize and reset)
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class BetterVectorAttribute : DrawnAttribute
    {
    }
}