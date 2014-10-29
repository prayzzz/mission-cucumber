using System;
using System.Linq;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Helpers;
using Vexe.Runtime.Types;

namespace Vexe.Editor.Framework.Drawers
{
	public class TypeDrawer : AttributeDrawer<Type, ShowTypeAttribute>
	{
		private Type[] availableTypes;
		private string[] typesNames;
		private int index;

		protected override void OnInitialized()
		{
			availableTypes = ReflectionHelper.GetAllTypesOf(attribute.baseType)
											         .Where(t => !t.IsAbstract)
											         .ToArray();

			typesNames = availableTypes.Select(t => t.Name)
									         .ToArray();
		}

		public override void OnGUI()
		{
			index = dataMember.IsNull() ? -1 : availableTypes.IndexOf(dmValue);
			var x = gui.Popup(niceName, index, typesNames);
			{
				if (index != x)
				{
					dmValue = availableTypes[x];
					index = x;
				}
			}
		}
	}
}