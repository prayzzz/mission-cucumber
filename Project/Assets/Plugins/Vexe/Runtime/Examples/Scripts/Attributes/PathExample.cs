using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Categories;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Decorates;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Others;
using Assets.Plugins.Vexe.Runtime.Types.Core;

namespace Assets.Plugins.Vexe.Runtime.Examples.Scripts.Attributes
{
    [BasicView]
    public class PathExample : BetterBehaviour
    {
        [Comment("Drag-drop a gameObject or asset to this field. " +
                    "Alternatively, you can hold Ctrl and do a middle mouse click on the field " +
                    "to show a selection window with all the gameObjects in the scene")]
        [Path]
        public string FullPath { get; set; }

        [Comment("There's a bug currently when you have more than one string and you drag-drop something, " +
                    "the value will always go to the first string - it puzzled me. Will fix. " +
                    "Check out PathAttributeDrawer if you want to have a look yourself")]
        [Path(false)]
        public string simplePath;

        [Path(AbsoluteAssetPath = true)]
        public string someAbsolutePath;
    }
}