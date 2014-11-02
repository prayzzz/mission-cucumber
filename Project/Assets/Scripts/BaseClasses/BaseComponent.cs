using Assets.Plugins.Vexe.Runtime.Types.Core;
using Assets.Scripts.Common;
using Assets.Scripts.Components;

using UnityEngine;

using Vexe.Runtime.Extensions;

namespace Assets.Scripts.BaseClasses
{
    [RequireComponent(typeof(BaseUnit))]
    public class BaseComponent : BetterBehaviour
    {
        protected BaseUnit Unit { get; private set; }

        protected TeamComponent TeamComponent { get; set; }

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
            this.TeamComponent = this.GetOrAddComponent<TeamComponent>();
        }

        protected float GetDistanceTo(BaseUnit unitInSight)
        {
            return Vector3.Distance(this.Unit.transform.position, unitInSight.transform.position);
        }
    }
}