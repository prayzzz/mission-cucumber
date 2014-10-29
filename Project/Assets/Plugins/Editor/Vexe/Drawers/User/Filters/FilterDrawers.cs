using System;
using System.Text.RegularExpressions;
using Smooth.Slinq;
using UnityEngine;
using Vexe.Editor.Framework.GUIs;
using Vexe.Editor.Helpers;
using Vexe.Runtime.Types;

namespace Vexe.Editor.Framework.Drawers
{
	public class FieldFilter
	{
		private readonly string[] values;
		private readonly GLWrapper gui;
		private Action<string> setValue;
		private string search;

		public FieldFilter(string[] values, GLWrapper gui, Action<string> setValue)
		{
			this.values   = values;
			this.gui      = gui;
			this.setValue = setValue;
		}

		public void OnGUI()
		{
			var newSearch = gui.TextField(GUIContent.none, search, GUILayout.Width(50f));
			if (search != newSearch)
			{
				var match = values.Slinq().FirstOrNone((n, p) =>
					p.Length == 1 ? p == n : Regex.IsMatch(n, p), newSearch);

				search = newSearch;

				if (match.isSome)
				{
					setValue(match.value);
				}
			}
		}
	}

	public abstract class FilterDrawer<T, A> : CompositeDrawer<T, A> where A : CompositeAttribute
	{
		private FieldFilter filter;

		protected override void OnInitialized()
		{
			filter = new FieldFilter(GetValues(), gui, SetValue);
		}

		public override void OnRightGUI()
		{
			filter.OnGUI();
		}

		protected abstract string[] GetValues();
		protected abstract void SetValue(string value);
	}

	public class FilterEnumAttributeDrawer : FilterDrawer<Enum, FilterEnumAttribute>
	{
		protected override void SetValue(string value)
		{
			dmValue = Enum.Parse(memberType, value) as Enum;
		}

		protected override string[] GetValues()
		{
			return Enum.GetNames(memberType);
		}
	}

	public class FilterTagsAttributeDrawer : FilterDrawer<string, FilterTagsAttribute>
	{
		protected override string[] GetValues()
		{
			return EditorHelper.Tags;
		}

		protected override void SetValue(string value)
		{
			dmValue = value;
		}
	}
}