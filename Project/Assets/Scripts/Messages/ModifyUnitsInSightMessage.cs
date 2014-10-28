using System.Collections.Generic;

using Assets.Scripts.BaseClasses;

namespace Assets.Scripts.Messages
{
    public class ModifyUnitsInSightMessage
    {
        public ModifyUnitsInSightMessage(IEnumerable<BaseUnit> unitsInSight)
        {
            this.UnitsInSight = new List<BaseUnit>(unitsInSight);
        }

        public IList<BaseUnit> UnitsInSight { get; private set; }
    }
}