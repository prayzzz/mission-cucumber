using Assets.Plugins.Vexe.Runtime.Types.Other;

using UnityEngine;

namespace Assets.Plugins.Vexe.Runtime.Examples.Scripts.Other
{
    public class EventManagerExample : MonoBehaviour
    {
        void ReportPlayerDeath(OnPlayerDied e)
        {
            print(string.Format("Player {0} has died because of {1}", e.Player.name, e.CauseOfDeath));
        }

        void RunTest1(OnTest e)
        {
            print("Test1: Float: " + e.FloatValue + " Int: " + e.IntValue);
        }

        void RunTest2(OnTest e)
        {
            print("Test2: Float: " + e.FloatValue + " Int: " + e.IntValue);
        }

        void OnEnable()
        {
            EventManager.Subscribe<OnPlayerDied>(this.ReportPlayerDeath);
        }

        void OnDisable()
        {
            EventManager.Unsubscribe<OnPlayerDied>(this.ReportPlayerDeath);
        }

        void KillPlayer()
        {
            EventManager.Raise(new OnPlayerDied { Player = this.transform, CauseOfDeath = "JustBecause" });
        }

        void OnGUI()
        {
            if (GUILayout.Button("Kill player"))
                this.KillPlayer();

            if (GUILayout.Button("Sub ReportPlayerDeath"))
                EventManager.Subscribe<OnPlayerDied>(this.ReportPlayerDeath);

            if (GUILayout.Button("Unsub ReportPlayerDeath"))
                EventManager.Unsubscribe<OnPlayerDied>(this.ReportPlayerDeath);

            if (GUILayout.Button("Is ReportPlayerDeath contained?"))
                print(EventManager.Contains<OnPlayerDied>(this.ReportPlayerDeath));

            if (GUILayout.Button("Clear OnPlayerDied"))
                EventManager.Clear<OnPlayerDied>();

            if (GUILayout.Button("Sub RunTest1"))
                EventManager.Subscribe<OnTest>(this.RunTest1);

            if (GUILayout.Button("Unsub RunTest1"))
                EventManager.Unsubscribe<OnTest>(this.RunTest1);

            if (GUILayout.Button("Sub RunTest2"))
                EventManager.Subscribe<OnTest>(this.RunTest2);

            if (GUILayout.Button("Unsub RunTest2"))
                EventManager.Unsubscribe<OnTest>(this.RunTest2);

            if (GUILayout.Button("Run tests"))
                EventManager.Raise(new OnTest { FloatValue = 1.3f, IntValue = 10 });

            if (GUILayout.Button("Raise to all except RunTest1"))
                EventManager.RaiseToAllExcept(new OnTest { FloatValue = 2.4f, IntValue = 20 }, this.RunTest1);

            if (GUILayout.Button("Is RunTest1 contained?"))
                print(EventManager.Contains<OnTest>(this.RunTest1));

            if (GUILayout.Button("Clear OnTest"))
                EventManager.Clear<OnTest>();
        }

        // Of course the events classes could be defined anywhere, not necessarily inside the same MonoBehaviour
        public class OnPlayerDied : GameEvent
        {
            public Transform Player { get; set; }
            public string CauseOfDeath { get; set; }
        }

        public class OnTest : GameEvent
        {
            public int IntValue { get; set; }
            public float FloatValue { get; set; }
        }
    }
}