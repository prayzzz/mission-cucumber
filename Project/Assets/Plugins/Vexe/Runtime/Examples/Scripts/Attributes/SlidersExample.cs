using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Categories;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Others;
using Assets.Plugins.Vexe.Runtime.Types.Core;

namespace Assets.Plugins.Vexe.Runtime.Examples.Scripts.Attributes
{
    [MinimalView]
    public class SlidersExample : BetterBehaviour
    {
        [FloatSlider(0, 10)]
        public float floatSlider;

        [IntSlider(3, 15)]
        public int IntSlider { get; set; }
    }
}