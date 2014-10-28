using Assets.Scripts.Common;

using UnityEngine;

namespace Assets.Scripts.BaseClasses
{
    [RequireComponent(typeof(BaseUnit))]
    public class BaseComponent : MonoBehaviour
    {
        public void Start()
        {
            this.Unit = this.GetComponent<BaseUnit>();
        }

        protected BaseUnit Unit { get; private set; }

        protected GameObjectMessenger Messenger
        {
            get
            {
                return this.Unit.Messenger;
            }
        }

    }
}