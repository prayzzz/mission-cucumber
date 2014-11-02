using System.Collections.Generic;
using System.Linq;

using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Constraints;
using Assets.Scripts.BaseClasses;
using Assets.Scripts.Messages;

using UnityEngine;

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
            var target = other.gameObject.GetComponent<BaseUnit>();

            if (target == null)
            {
                return;
            }

            var isVisible = new IsVisibleMessage();
            target.Messenger.Send(isVisible);

            var isAlive = new IsAliveMessage();
            target.Messenger.Send(isAlive);

            if (isVisible.IsVisble && isAlive.IsAlive)
            {
                this.unitsInSight.Add(target);
            }
        }

        public void Update()
        {
            if (this.unitsInSight.Any())
            {
                var modifyMessage = new ModifyUnitsInSightMessage(this.unitsInSight);
                this.Messenger.Send(modifyMessage);

                var publishMessage = new UnitsInSightMessage(modifyMessage.UnitsInSight);
                this.Messenger.Send(publishMessage);
            }

            this.unitsInSight.Clear();
        }
    }
}