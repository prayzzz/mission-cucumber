using Assets.Scripts.BaseClasses;
using Assets.Scripts.Messages;

namespace Assets.Scripts.Components
{
    public class HealthComponent : BaseComponent
    {
        public HealthComponent(int totalHealth)
        {
            this.TotalHealth = totalHealth;
            this.CurrentHealth = totalHealth;

            this.RegisterEventHandler();
        }

        ~HealthComponent()
        {
            this.UnregisterEventHandler();
        }

        public int CurrentHealth { get; set; }

        public int TotalHealth { get; set; }

        public bool IsDead
        {
            get
            {
                return this.CurrentHealth <= 0;
            }
        }

        private void RegisterEventHandler()
        {
            this.Messenger.Register<ReceiveHealingMessage>(this, this.ReceiveHealing);
            this.Messenger.Register<TakeDamageMessage>(this, this.TakeDamage);
        }

        private void UnregisterEventHandler()
        {
            this.Messenger.Unregister<ReceiveHealingMessage>(this.ReceiveHealing);
            this.Messenger.Unregister<TakeDamageMessage>(this.TakeDamage);
        }

        private void TakeDamage(TakeDamageMessage message)
        {
            var damageMessage = new ModifyIncomingDamageMessage(message.Damage);
            Messenger.Send(damageMessage);

            this.Damage(damageMessage.Damage);
        }

        private void Damage(int amount)
        {
            if (this.IsDead)
            {
                return;
            }

            this.CurrentHealth -= amount;

            this.Messenger.Send(new DamageTakenMessage(amount));

            if (this.CurrentHealth <= 0)
            {
                this.Messenger.Send(new UnitDiedMessage(this.Unit));
            }
        }

        private void ReceiveHealing(ReceiveHealingMessage message)
        {
            var healingMessage = new ModifyIncomingHealingMessage(message.Healing);
            this.Messenger.Send(healingMessage);

            this.Heal(healingMessage.Healing);
        }

        private void Heal(int amount)
        {
            if (this.IsDead)
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