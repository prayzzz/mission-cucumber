using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.BaseClasses;
using Assets.Scripts.Messages;

using UnityEngine;

using Vexe.Runtime.Types;

namespace Assets.Scripts.Components
{
    public class SightComponent : BaseComponent
    {
        private HashSet<BaseUnit> unitsInSight;

        [Min(0)]
        public int SightRadius { get; set; }
        
        public override void Awake()
        {
            base.Awake();

            this.unitsInSight = new HashSet<BaseUnit>();

            var trigger = this.gameObject.AddComponent<SphereCollider>();
            trigger.radius = this.SightRadius;
            trigger.isTrigger = true;
        }

        public void OnTriggerStay(Collider other)
        {
            var unit = other.gameObject.GetComponent<BaseUnit>();

            if (unit == null)
            {
                return;
            }

            var isVisible = new IsVisibleMessage();

            unit.Messenger.Send(isVisible);

            if (isVisible.IsVisble)
            {
                this.unitsInSight.Add(unit);
            }
        }

        public void Update()
        {
            if (this.unitsInSight.Any())
            {
                var modifyMessage = new ModifyUnitsInSightMessage(this.unitsInSight);
                this.Unit.Messenger.Send(modifyMessage);

                var publishMessage = new UnitsInSightMessage(modifyMessage.UnitsInSight);
                this.Unit.Messenger.Send(publishMessage);
            }

            this.unitsInSight.Clear();
        }
    }
}