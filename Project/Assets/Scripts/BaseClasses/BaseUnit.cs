using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Categories;
using Assets.Plugins.Vexe.Runtime.Types.Core;
using Assets.Scripts.Common;
using Assets.Scripts.Messages;

using UnityEngine;

namespace Assets.Scripts.BaseClasses
{
    [BasicView]
    public class BaseUnit : BetterBehaviour
    {
        public TeamEnum Team { get; set; }

        public GameObjectMessenger Messenger { get; private set; }

        public void Start()
        {
            this.RegisterEventHandler();
        }

        public void Awake()
        {
            this.Messenger = new GameObjectMessenger();
        }

        public float GetDistanceTo(BaseUnit unitInSight)
        {
            return Vector3.Distance(this.transform.position, unitInSight.transform.position);
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