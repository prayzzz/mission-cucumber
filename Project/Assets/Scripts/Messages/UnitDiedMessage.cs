using Assets.Scripts.BaseClasses;

namespace Assets.Scripts.Messages
{
    public class UnitDiedMessage
    {
        public UnitDiedMessage(BaseUnit unit)
        {
            this.Unit = unit;
        }

        public BaseUnit Unit { get; private set; }
    }
}