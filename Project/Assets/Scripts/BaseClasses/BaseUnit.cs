using Assets.Scripts.Common;

using Vexe.Runtime.Types;

namespace Assets.Scripts.BaseClasses
{
    public class BaseUnit : BetterBehaviour
    {
        public BaseUnit()
        {
            this.Messenger = new GameObjectMessenger();
        }

        public GameObjectMessenger Messenger { get; private set; }
    }
}