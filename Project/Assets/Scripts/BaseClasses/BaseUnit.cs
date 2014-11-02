using Assets.Plugins.Vexe.Runtime.Types.Core;
using Assets.Scripts.Common;

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