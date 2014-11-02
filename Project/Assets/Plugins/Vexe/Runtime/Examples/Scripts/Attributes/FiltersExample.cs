using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Filters;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Popups;
using Assets.Plugins.Vexe.Runtime.Types.Core;

using UnityEngine;

namespace Assets.Plugins.Vexe.Runtime.Examples.Scripts.Attributes
{
    public class FiltersExample : BetterBehaviour
    {
        [FilterEnum]
        public KeyCode jumpKey;

        [FilterTags]
        public string playerTag;

        [Tags, FilterTags]
        public string enemyTag;
    }
}