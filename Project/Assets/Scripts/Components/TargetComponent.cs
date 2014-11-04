using System;
using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.BaseClasses;
using Assets.Scripts.Components.TargetFilter;
using Assets.Scripts.Messages;

namespace Assets.Scripts.Components
{
    [Serializable]
    public class TargetComponent
    {
        private readonly BaseUnit unit;

        private readonly Action<IEnumerable<BaseUnit>> onNewTargets;

        public TargetComponent(BaseUnit unit, Action<IEnumerable<BaseUnit>> onNewTargets)
        {
            this.Filters = new List<BaseTargetFilter>();

            this.onNewTargets = onNewTargets;
            this.unit = unit;
        }

        public List<BaseTargetFilter> Filters { get; set; }

        public void Start()
        {
            foreach (var targetFilter in this.Filters)
            {
                targetFilter.Unit = this.unit;
            }

            this.RegisterEventHandler();
        }

        private void RegisterEventHandler()
        {
            this.unit.Messenger.Register<UnitsInSightMessage>(this, this.OnUnitsInSight);
        }

        private void UnregisterEventHandler()
        {
            this.unit.Messenger.Unregister<UnitsInSightMessage>(this.OnUnitsInSight);
        }

        private void OnUnitsInSight(UnitsInSightMessage message)
        {
            if (!message.UnitsInSight.Any())
            {
                return;
            }

            // Applies continuously all assigned filters on UnitsInSight
            var filteredTargets = this.Filters.Aggregate(message.UnitsInSight, (current, targetFilter) => current.Where(targetFilter.IsTargetValid));
            filteredTargets = filteredTargets.OrderBy(target => this.unit.GetDistanceTo(target));

            if (filteredTargets.Any() && this.onNewTargets != null)
            {
                this.onNewTargets(filteredTargets);
            }
        }
    }
}