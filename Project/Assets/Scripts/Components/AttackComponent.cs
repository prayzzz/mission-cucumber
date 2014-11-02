using System;

using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Constraints;
using Assets.Scripts.BaseClasses;
using Assets.Scripts.Common;
using Assets.Scripts.Messages;

using Vexe.Runtime.Extensions;

namespace Assets.Scripts.Components
{
    public class AttackComponent : BaseComponent
    {
        [Min(0)]
        public int AttackRange { get; set; }

        [Min(0)]
        public int AttackDamage { get; set; }

        public TargetTypeEnum TargetType { get; set; }

        public BaseUnit Target { get; private set; }

        public SingleTargetComponent SingleTargetComponent { get; private set; }

        public override void Awake()
        {
            base.Awake();

            this.SingleTargetComponent = this.GetOrAddComponent<SingleTargetComponent>();
            this.SingleTargetComponent.OnNewTarget = this.OnNewTarget;
            this.SingleTargetComponent.TargetType = this.TargetType;

            this.RegisterEventHandler();
        }

        private bool OnNewTarget(BaseUnit target)
        {
            if (this.GetDistanceTo(target) > this.AttackRange)
            {
                return false;
            }

            this.Target = target;
            this.InvokeRepeating("Attack", 1, 1);
            return true;
        }

        private void RegisterEventHandler()
        {
            this.Messenger.Register<UnitDiedMessage>(this, this.OnUnitDied);
        }

        private void UnregisterEventHandler()
        {
            this.Messenger.Unregister<UnitDiedMessage>(this.OnUnitDied);
        }

        private void OnUnitDied(UnitDiedMessage message)
        {
            this.CancelInvoke("Attack");

            this.Target = null;
            this.SingleTargetComponent.Clear();
        }

        [UsedImplicitly]
        private void Attack()
        {
            if (this.Target == null)
            {
                return;
            }

            var attackMessage = new ModifyOutgoingDamageMessage(this.AttackDamage);
            this.Messenger.Send(attackMessage);

            Log("{0} - {1} deals {2} damage to {3}", DateTime.Now.TimeOfDay, this, attackMessage.Damage, this.Target);
            this.Target.Messenger.Send(new TakeDamageMessage(this.Unit, attackMessage.Damage));

            this.Messenger.Send(new DamageDoneMessage(attackMessage.Damage));
        }
    }
}