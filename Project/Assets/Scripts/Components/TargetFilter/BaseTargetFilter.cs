using System;

using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;
using Assets.Scripts.BaseClasses;

namespace Assets.Scripts.Components.TargetFilter
{
    [Serializable]
    public abstract class BaseTargetFilter
    {
        [Hide]
        public BaseUnit Unit { get; set; }

        public abstract bool IsTargetValid(BaseUnit target);
    }
}