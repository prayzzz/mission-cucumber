using Assets.Scripts.Common;

using UnityEngine;

using Vexe.Runtime.Types;

namespace Assets.Scripts.BaseClasses
{
    [RequireComponent(typeof(BaseUnit))]
    public class BaseComponent : BetterBehaviour
    {
        protected BaseUnit Unit { get; private set; }

        protected GameObjectMessenger Messenger
        {
            get
            {
                return this.Unit.Messenger;
            }
        }

        public virtual void Awake()
        {
            this.Unit = this.GetComponent<BaseUnit>();
        }
    }
}