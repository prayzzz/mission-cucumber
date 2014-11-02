using System;
using System.Text.RegularExpressions;

using Assets.Plugins.Editor.Vexe.Drawers.API.Base;
using Assets.Plugins.Editor.Vexe.GUI;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Filters;

using Smooth.Slinq;

using UnityEngine;

using Vexe.Editor.Helpers;

namespace Assets.Plugins.Editor.Vexe.Drawers.User.Filters
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
            var newSearch = this.gui.TextField(GUIContent.none, this.search, GUILayout.Width(50f));
            if (this.search != newSearch)
            {
                var match = this.values.Slinq().FirstOrNone((n, p) =>
                    p.Length == 1 ? p == n : Regex.IsMatch(n, p), newSearch);

                this.search = newSearch;

                if (match.isSome)
                {
                    this.setValue(match.value);
                }
            }
        }
    }

    public abstract class FilterDrawer<T, A> : CompositeDrawer<T, A> where A : CompositeAttribute
    {
        private FieldFilter filter;

        protected override void OnInitialized()
        {
            this.filter = new FieldFilter(this.GetValues(), this.gui, this.SetValue);
        }

        public override void OnRightGUI()
        {
            this.filter.OnGUI();
        }

        protected abstract string[] GetValues();
        protected abstract void SetValue(string value);
    }

    public class FilterEnumAttributeDrawer : FilterDrawer<Enum, FilterEnumAttribute>
    {
        protected override void SetValue(string value)
        {
            this.dmValue = Enum.Parse(this.memberType, value) as Enum;
        }

        protected override string[] GetValues()
        {
            return Enum.GetNames(this.memberType);
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
            this.dmValue = value;
        }
    }
}