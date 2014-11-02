using System.Collections.Generic;

using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Categories;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Popups;
using Assets.Plugins.Vexe.Runtime.Types.Core;

using UnityEngine;

namespace Assets.Plugins.Vexe.Runtime.Examples.Scripts.Attributes
{
    [BasicView]
    public class AnimVarExample : BetterBehaviour
    {
        [AnimVar(AutoMatch = "Var")] // this will try to auto-assign "Speed" to this field
        public string speedVar;

        [PerKey, AnimVar]
        public Dictionary<string, GameObject> Variables { get; set; }

        [AnimVar(GetAnimator = "GetAnim")]
        public string anotherVar;

        private Animator GetAnim()
        {
            // well this is pointless because it would attempt to get the animator from this gameObject
            // by default without having us to specify a method.
            // but imagine we're getting an animtor from somewhere else...
            return this.GetComponent<Animator>();
        }
    }
}