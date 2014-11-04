using Assets.Scripts.BaseClasses;
using Assets.Scripts.Common;

namespace Assets.Scripts.Components.TargetFilter
{
    public class AllyFilter : BaseTargetFilter
    {
        public override bool IsTargetValid(BaseUnit target)
        {
            if (target.Team == TeamEnum.Neutral)
            {
                return false;
            }

            return target.Team == this.Unit.Team;
        }
    }
}