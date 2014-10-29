using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.BaseClasses;
using Assets.Scripts.Messages;

using UnityEngine;

namespace Assets.Scripts.Components
{
    public class SightComponent : BaseComponent
    {
        public int SightRadius;

        private HashSet<BaseUnit> unitsInSight;

        public void Awake()
        {
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