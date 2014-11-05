using System;
using System.Collections.Generic;
using System.Linq;

using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Categories;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Constraints;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Decorates;
using Assets.Scripts.BaseClasses;
using Assets.Scripts.Common;
using Assets.Scripts.Messages;

namespace Assets.Scripts.Components
{
    [BasicView]
    public class MeleeAttackComponent : BaseComponent
    {
        private BaseUnit target;

        private RepeatingInvoker invoker;

        [Min(0)]
        public int AttackRange { get; set; }

        [Min(0)]
        public int AttackDamage { get; set; }

        [Min(0)]
        [Comment("In Sekunden")]
        public float AttackSpeed { get; set; }

        public TargetComponent TargetComponent { get; set; }

        public override void Awake()
        {
            base.Awake();

            this.TargetComponent = new TargetComponent(this.Unit, this.OnNewTargets);
            this.invoker = new RepeatingInvoker(AttackSpeed * 1000, this.Attack);
        }

        public override void Start()
        {
            base.Start();

            this.TargetComponent.Start();
            this.RegisterEventHandler();
        }

        private void OnNewTargets(IEnumerable<BaseUnit> targets)
        {
            // No possible targets
            if (!targets.Any())
            {
                this.ClearTarget();
                return;
            }

            // Current target still in list
            if (targets.Any(t => this.target == t))
            {
                return;
            }

            // No target in range
            var newTarget = targets.First();
            if (this.Unit.GetDistanceTo(newTarget) > this.AttackRange)
            {
                this.ClearTarget();
                return;
            }

            // New target
            this.target = newTarget;
            if (!this.invoker.IsRunning)
            {
                this.invoker.Start();
            }
        }

        private void RegisterEventHandler()
        {
        }

        private void UnregisterEventHandler()
        {
        }

        private void ClearTarget()
        {
            this.target = null;
            this.invoker.Stop();
        }

        [UsedImplicitly]
        private void Attack()
        {
            if (this.target == null)
            {
                return;
            }

            var attackMessage = new ModifyOutgoingDamageMessage(this.AttackDamage);
            this.Messenger.Send(attackMessage);

            Log("{0} - {1} deals {2} damage to {3}", DateTime.Now.TimeOfDay, this, attackMessage.Damage, this.target);
            this.target.Messenger.Send(new DealDamageMessage(attackMessage.Damage));

            this.Messenger.Send(new DamageDoneMessage(attackMessage.Damage));
        }
    }
}