using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.BaseClasses;
using Assets.Scripts.Messages;

using UnityEngine;

namespace Assets.Scripts.Components
{
    [RequireComponent(typeof(SphereCollider))]
    public class SightComponent : BaseComponent
    {
        private HashSet<BaseUnit> unitsInSight;

        public override void Awake()
        {
            base.Awake();

            this.unitsInSight = new HashSet<BaseUnit>();
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

        public void OnTriggerExit(Collider other)
        {
            var unit = other.gameObject.GetComponent<BaseUnit>();

            if (unit == null)
            {
                return;
            }

            this.unitsInSight.Remove(unit);
        }

        public void Update()
        {
            if (!this.unitsInSight.Any())
            {
                return;
            }

            var modifyMessage = new ModifyUnitsInSightMessage(this.unitsInSight);
            this.Unit.Messenger.Send(modifyMessage);

            var publishMessage = new UnitsInSightMessage(modifyMessage.UnitsInSight);
            this.Unit.Messenger.Send(publishMessage);
        }
    }
}