using System.Collections.Generic;

using Assets.Plugins.Vexe.Runtime.Examples.Scripts.Serialization;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;
using Assets.Plugins.Vexe.Runtime.Types.Core;
using Assets.Plugins.Vexe.Runtime.Types.Delegates;

using UnityEngine;

namespace Assets.Plugins.Vexe.Runtime.Examples.Scripts.Delegates
{
    public class DelegatesExample : BetterBehaviour
    {
        public uAction action0;
        public uAction<string> Action1 { get; set; } 
        public uAction<GameObject, Vector3> action2;
        public uAction<ITest, Transform, float> Action3 { get; set; }

        public uFunc<string> func0;
        public uFunc<int, bool> Func1 { get; set; }
        public uFunc<GameObject, Vector3, string> func2;
        public uFunc<ITest, Transform, float, string> Func3 { get; set; }

        public uAction[] actions0;
        public List<uAction<string>> actions1;
        public Dictionary<string, List<uAction>> dict;

        [Hide]
        public void Action0Method0()
        {
            Log("Action0Method0");

            this.action0.Add(this.Action0Method1);
            this.action0.Invoke();
        }

        public void Action0Method1()
        {
            Log("Action0Method1");
        }
        public void Action1Method(string arg0)
        {
            Log("Action1Method {0}", arg0);
        }
        public void Action2Method(GameObject arg0, Vector3 arg1)
        {
            Log("Action0Method {0}, {1}", arg0, arg1);
        }
        public void Action3Method(ITest arg0, Transform arg1, float arg2)
        {
            Log("Action1Method {0}, {1}, {2}", arg0, arg1, arg2);
        }

        public string Func0Method()
        {
            Log("Func0Method");
            return string.Empty;
        }
        public bool Func1Method(int arg0)
        {
            Log("Func1Method {0}", arg0);
            return arg0 > 0;
        }
        public string Func2Method(GameObject arg0, Vector3 arg1)
        {
            Log("Func0Method {0}, {1}", arg0, arg1);
            return arg1.ToString();
        }
        public string Func3Method(ITest arg0, Transform arg1, float arg2)
        {
            Log("Func1Method {0}, {1}, {2}", arg0, arg1, arg2);
            return arg2.ToString();
        }
    }
}