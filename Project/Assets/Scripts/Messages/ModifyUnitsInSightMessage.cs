using System.Collections.Generic;

using Assets.Scripts.BaseClasses;

namespace Assets.Scripts.Messages
{
    public class ModifyUnitsInSightMessage
    {
        public IList<BaseUnit> UnitsInSight { get; private set; }

        public ModifyUnitsInSightMessage(IEnumerable<BaseUnit> unitsInSight)
        {
            this.UnitsInSight = new List<BaseUnit>(unitsInSight);
        }
    }
}