using System;
using System.Linq;
using Fasterflect;
using UnityEngine;
using Vexe.Runtime.Exceptions;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Vexe.Editor.Framework.Drawers
{
	public abstract class BasePopupAttributeDrawer<T> : AttributeDrawer<T, PopupAttribute>
	{
		private string[] values;
		private int? currentIndex;
		private MethodInvoker populate;

		protected override void OnInitialized()
		{
			string populateMethod = attribute.PopulateFrom;
			if (populateMethod.IsNullOrEmpty())
			{
				values = attribute.values;
			}
			else
			{
				populate = targetType.DelegateForCallMethod(populateMethod);
				if (populate == null)
					throw new MemberNotFoundException(populateMethod);
				values = ProcessPopulation(populate(rawTarget));
			}
		}

		Func<T, string> _getString;
		Func<T, string> GetString()
		{
			return _getString ?? (_getString = new Func<T, string>(x => x.ToString()).Memoize());
		}

		public override void OnGUI()
		{
			if (!currentIndex.HasValue)
			{
				string currentValue = GetString().Invoke(dmValue);
				currentIndex = Mathf.Max(0, values.IndexOf(currentValue));
			}

			int x = gui.Popup(niceName, currentIndex.Value, values);
			{
				if (currentIndex != x)
				{
					SetValue(values[x]);
					currentIndex = x;
				}
			}
		}

		protected abstract string[] ProcessPopulation(object population);
		protected abstract void SetValue(string value);
	}

	public class IntPopupAttributeDrawer : BasePopupAttributeDrawer<int>
	{
		protected override string[] ProcessPopulation(object population)
		{
			return (population as int[]).Select(x => x.ToString()).ToArray();
		}

		protected override void SetValue(string value)
		{
			dmValue = Convert.ToInt32(value);
		}
	}
	public class FloatPopupAttributeDrawer : BasePopupAttributeDrawer<float>
	{
		protected override string[] ProcessPopulation(object population)
		{
			return (population as float[]).Select(x => x.ToString()).ToArray();
		}

		protected override void SetValue(string value)
		{
			dmValue = Convert.ToSingle(value);
		}
	}
	public class StringPopupAttributeDrawer : BasePopupAttributeDrawer<string>
	{
		protected override void OnInitialized()
		{
			base.OnInitialized();
			if (dmValue == null)
				dmValue = string.Empty;
		}

		protected override string[] ProcessPopulation(object population)
		{
			return (population as string[]).Select(x => x.ToString()).ToArray();
		}

		protected override void SetValue(string value)
		{
			dmValue = value;
		}
	}
}