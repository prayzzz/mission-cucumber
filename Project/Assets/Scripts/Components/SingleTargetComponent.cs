using System;
using System.Linq;

using Assets.Scripts.BaseClasses;
using Assets.Scripts.Common;
using Assets.Scripts.Messages;

namespace Assets.Scripts.Components
{
    public class SingleTargetComponent : BaseComponent
    {
        public Func<BaseUnit, bool> OnNewTarget { get; set; }

        public TargetTypeEnum TargetType { get; set; }

        public BaseUnit CurrentTarget { get; private set; }

        public BaseUnit NewTarget { get; private set; }

        public override void Awake()
        {
            base.Awake();

            this.RegisterEventHandler();
        }

        public void Update()
        {
            if (this.NewTarget != this.CurrentTarget && this.OnNewTarget(this.NewTarget))
            {
                this.CurrentTarget = this.NewTarget;
            }
        }

        public void Clear()
        {
            this.CurrentTarget = null;
            this.NewTarget = null;
        }

        private void RegisterEventHandler()
        {
            this.Messenger.Register<UnitsInSightMessage>(this, this.OnUnitsInSight);
        }

        private void UnregisterEventHandler()
        {
            this.Messenger.Unregister<UnitsInSightMessage>(this.OnUnitsInSight);
        }

        private void OnUnitsInSight(UnitsInSightMessage message)
        {
            if (!message.UnitsInSight.Any())
            {
                return;
            }

            if (this.CurrentTarget == null)
            {
                this.NewTarget = this.GetNearestUnit(message);
                return;
            }

            if (!message.UnitsInSight.Contains(this.CurrentTarget))
            {
                this.NewTarget = this.GetNearestUnit(message);
            }
        }

        private BaseUnit GetNearestUnit(UnitsInSightMessage message)
        {
            BaseUnit target = null;
            var minDistance = float.MaxValue;

            foreach (var unitInSight in message.UnitsInSight)
            {
                var distance = this.GetDistanceTo(unitInSight);
                if (distance < minDistance && this.IsValidTarget(unitInSight))
                {
                    minDistance = distance;
                    target = unitInSight;
                }
            }

            return target;
        }

        private bool IsValidTarget(BaseUnit target)
        {
            var teamRequest = new TeamRequestMessage();
            target.Messenger.Send(teamRequest);

            if (teamRequest.Team == TeamEnum.Neutral)
            {
                return false;
            }

            if (TargetType == TargetTypeEnum.Ally && teamRequest.Team == this.TeamComponent.Team)
            {
                return true;
            }

            if (TargetType == TargetTypeEnum.Enemy && teamRequest.Team != this.TeamComponent.Team)
            {
                return true;
            }

            return false;
        }
    }
}