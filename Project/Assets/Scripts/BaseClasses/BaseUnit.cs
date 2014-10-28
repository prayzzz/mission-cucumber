using Assets.Scripts.Common;

using UnityEngine;

namespace Assets.Scripts.BaseClasses
{
    public class BaseUnit : MonoBehaviour
    {
        public GameObjectMessenger Messenger { get; private set; }

        public BaseUnit()
        {
            this.Messenger = new GameObjectMessenger();
        }
    }
}