using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Categories;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Decorates;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Others;
using Assets.Plugins.Vexe.Runtime.Types.Core;

using UnityEngine;

namespace Assets.Plugins.Vexe.Runtime.Examples.Scripts.Attributes
{
    [BasicView]
    public class ReadonlyExample : BetterBehaviour
    {
        [Comment("You can't assign it from the inspector even during edit-time (can only be assigned from code)")]
        [Readonly]
        public Transform ReadonlyAtEditAndRuntime { get; set; }

        [Comment("You can only assign this during editing")]
        [Readonly(AssignAtEditTime = true)]
        public Transform readonlyAtRuntime;
    }
}