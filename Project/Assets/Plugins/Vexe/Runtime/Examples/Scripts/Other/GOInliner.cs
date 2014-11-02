using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Categories;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Others;
using Assets.Plugins.Vexe.Runtime.Types.Core;

using UnityEngine;

namespace Assets.Plugins.Vexe.Runtime.Examples.Scripts.Other
{
    /// <summary>
    /// A convenient script to delegate the drawing of another gameObject
    /// Useful if you have a deep hierarchy of objects, and the object that you're most interested in
    /// is deep down, so you just attach this script to the top-most object,
    /// and have it reference the one you're interested in
    /// </summary>
    [BasicView]
    public class GOInliner : BetterBehaviour
    {
        [Inline]
        public GameObject target;
    }
}