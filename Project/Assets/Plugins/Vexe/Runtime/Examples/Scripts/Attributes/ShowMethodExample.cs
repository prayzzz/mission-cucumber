using Assets.Plugins.Vexe.Runtime.Examples.Scripts.Serialization;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Categories;
using Assets.Plugins.Vexe.Runtime.Types.Core;

using UnityEngine;

namespace Assets.Plugins.Vexe.Runtime.Examples.Scripts.Attributes
{
    /// <summary>
    /// A small demo for exposing methods in the inspector
    /// Notes: The arguments you set to the method in the editor won't survive assembly reloads
    /// </summary>
    [BasicView]
    public class ShowMethodExample : BetterBehaviour
    {
        [Show]
        public void Method0()
        {
            Log("void - no params");
        }

        [Show]
        public void Method1(string arg0)
        {
            this.name = arg0;
        }

        [Show]
        public int Method2(Transform arg0, ITest arg1)
        {
            Log("returning " + arg0.name + " children count: " + arg0.childCount);
            Log(arg1.Vector);
            return arg0.childCount;
        }
    }
}