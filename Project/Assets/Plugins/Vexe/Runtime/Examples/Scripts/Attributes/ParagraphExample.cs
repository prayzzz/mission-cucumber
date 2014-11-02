using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Categories;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Others;
using Assets.Plugins.Vexe.Runtime.Types.Core;

namespace Assets.Plugins.Vexe.Runtime.Examples.Scripts.Attributes
{
    [MinimalView]
    public class ParagraphExample : BetterBehaviour
    {
        [Paragraph]
        public string p1;

        [Paragraph]
        public string P2 { get; set; }
    }
}