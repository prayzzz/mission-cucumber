using System;

using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Constraints;
using Assets.Scripts.BaseClasses;
using Assets.Scripts.Messages;

namespace Assets.Scripts.Components
{
    public class HealthComponent : BaseComponent
    {
        [Min(0)]
        public int TotalHealth { get; set; }

        public int CurrentHealth { get; private set; }

        public bool IsAlive
        {
            get
            {
                return this.CurrentHealth > 0;
            }
        }

        public override void Awake()
        {
            base.Awake();

            this.CurrentHealth = this.TotalHealth;

            this.RegisterEventHandler();
        }

        private void RegisterEventHandler()
        {
            this.Messenger.Register<ReceiveHealingMessage>(this, this.OnReceiveHealing);
            this.Messenger.Register<TakeDamageMessage>(this, this.OnTakeDamage);
            this.Messenger.Register<IsAliveMessage>(this, this.OnIsAlive);
        }

        private void UnregisterEventHandler()
        {
            this.Messenger.Unregister<ReceiveHealingMessage>(this.OnReceiveHealing);
            this.Messenger.Unregister<TakeDamageMessage>(this.OnTakeDamage);
            this.Messenger.Unregister<IsAliveMessage>(this.OnIsAlive);
        }

        private void OnIsAlive(IsAliveMessage message)
        {
            message.IsAlive = this.IsAlive;
        }

        private void OnTakeDamage(TakeDamageMessage message)
        {
            var damageMessage = new ModifyIncomingDamageMessage(message.Damage);
            this.Messenger.Send(damageMessage);

            this.TakeDamage(message.Sender, damageMessage.Damage);
        }

        private void TakeDamage(BaseUnit attacker, int amount)
        {
            if (!this.IsAlive)
            {
                return;
            }

            this.CurrentHealth -= amount;

            this.Messenger.Send(new DamageTakenMessage(amount));

            if (this.CurrentHealth <= 0)
            {
                this.UnitDied(attacker);
            }
        }

        private void UnitDied(BaseUnit attacker)
        {
            attacker.Messenger.Send(new UnitDiedMessage(this.Unit));

            Log("{0} - {1} died to {2}", DateTime.Now.TimeOfDay, this, attacker);

            Destroy(this.gameObject);
        }

        private void OnReceiveHealing(ReceiveHealingMessage message)
        {
            var healingMessage = new ModifyIncomingHealingMessage(message.Healing);
            this.Messenger.Send(healingMessage);

            this.ReceiveHeal(healingMessage.Healing);
        }

        private void ReceiveHeal(int amount)
        {
            if (!this.IsAlive)
            {
                return;
            }

            this.CurrentHealth += amount;

            if (this.CurrentHealth > this.TotalHealth)
            {
                this.CurrentHealth = this.TotalHealth;
            }

            this.Messenger.Send(new HealingReceivedMessage(amount));
        }
    }
}