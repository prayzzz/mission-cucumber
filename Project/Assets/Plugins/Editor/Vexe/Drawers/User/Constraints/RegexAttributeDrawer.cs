using System.Text.RegularExpressions;

using Assets.Plugins.Editor.Vexe.Drawers.API.Base;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Constraints;

namespace Assets.Plugins.Editor.Vexe.Drawers.User.Constraints
{
    public class RegexAttributeDrawer<T> : CompositeDrawer<string, T> where T : RegexAttribute
    {
        private string recentValid;

        public override void OnUpperGUI() // doesn't really matter much which section we override.. we're just modifying the member value and not drawing anything
        {
            string current = this.dmValue ?? string.Empty;

            if (Regex.IsMatch(current, this.attribute.pattern))
            {
                this.recentValid = current;
            }
            else
            {
                this.dmValue = this.recentValid;
            }
        }
    }
}