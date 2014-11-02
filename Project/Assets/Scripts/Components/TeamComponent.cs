using Assets.Scripts.BaseClasses;
using Assets.Scripts.Common;
using Assets.Scripts.Messages;

namespace Assets.Scripts.Components
{
    public class TeamComponent : BaseComponent
    {
        public TeamEnum Team { get; set; }

        public override void Awake()
        {
            base.Awake();

            this.RegisterEventHandler();
        }

        private void RegisterEventHandler()
        {
            this.Messenger.Register<TeamRequestMessage>(this, this.OnTeamRequest);
        }

        private void UnregisterEventHandler()
        {
            this.Messenger.Unregister<TeamRequestMessage>(this.OnTeamRequest);
        }

        private void OnTeamRequest(TeamRequestMessage message)
        {
            message.Team = this.Team;
        }
    }
}
