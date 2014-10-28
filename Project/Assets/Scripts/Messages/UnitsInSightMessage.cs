using System.Collections.Generic;

using Assets.Scripts.BaseClasses;

namespace Assets.Scripts.Messages
{
    public class UnitsInSightMessage
    {
        public UnitsInSightMessage(IEnumerable<BaseUnit> unitsInSight)
        {
            this.UnitsInSight = unitsInSight;
        }

        public IEnumerable<BaseUnit> UnitsInSight { get; private set; }
    }
}