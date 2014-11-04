using System;

using Assets.Scripts.BaseClasses;
using Assets.Scripts.Common;

namespace Assets.Scripts.Components.TargetFilter
{
    [Serializable]
    public class EnemyFilter : BaseTargetFilter
    {
        public override bool IsTargetValid(BaseUnit target)
        {
            if (target.Team == TeamEnum.Neutral)
            {
                return false;
            }

            return target.Team != this.Unit.Team;
        }
    }
}