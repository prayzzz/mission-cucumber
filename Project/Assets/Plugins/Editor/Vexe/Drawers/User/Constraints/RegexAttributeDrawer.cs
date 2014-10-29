using System.Text.RegularExpressions;
using Vexe.Runtime.Types;

namespace Vexe.Editor.Framework.Drawers
{
	public class RegexAttributeDrawer<T> : CompositeDrawer<string, T> where T : RegexAttribute
	{
		private string recentValid;

		public override void OnUpperGUI() // doesn't really matter much which section we override.. we're just modifying the member value and not drawing anything
		{
			string current = dmValue ?? string.Empty;

			if (Regex.IsMatch(current, attribute.pattern))
			{
				recentValid = current;
			}
			else
			{
				dmValue = recentValid;
			}
		}
	}
}