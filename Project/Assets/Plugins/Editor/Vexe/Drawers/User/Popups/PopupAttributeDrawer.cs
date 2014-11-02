using System;
using System.Linq;

using Assets.Plugins.Editor.Vexe.Drawers.API.Base;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Popups;

using Fasterflect;

using UnityEngine;

using Vexe.Runtime.Exceptions;
using Vexe.Runtime.Extensions;

namespace Assets.Plugins.Editor.Vexe.Drawers.User.Popups
{
    public abstract class BasePopupAttributeDrawer<T> : AttributeDrawer<T, PopupAttribute>
    {
        private string[] values;
        private int? currentIndex;
        private MethodInvoker populate;

        protected override void OnInitialized()
        {
            string populateMethod = this.attribute.PopulateFrom;
            if (populateMethod.IsNullOrEmpty())
            {
                this.values = this.attribute.values;
            }
            else
            {
                this.populate = this.targetType.DelegateForCallMethod(populateMethod);
                if (this.populate == null)
                    throw new MemberNotFoundException(populateMethod);
                this.values = this.ProcessPopulation(this.populate(this.rawTarget));
            }
        }

        Func<T, string> _getString;
        Func<T, string> GetString()
        {
            return this._getString ?? (this._getString = new Func<T, string>(x => x.ToString()).Memoize());
        }

        public override void OnGUI()
        {
            if (!this.currentIndex.HasValue)
            {
                string currentValue = this.GetString().Invoke(this.dmValue);
                this.currentIndex = Mathf.Max(0, this.values.IndexOf(currentValue));
            }

            int x = this.gui.Popup(this.niceName, this.currentIndex.Value, this.values);
            {
                if (this.currentIndex != x)
                {
                    this.SetValue(this.values[x]);
                    this.currentIndex = x;
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
            this.dmValue = Convert.ToInt32(value);
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
            this.dmValue = Convert.ToSingle(value);
        }
    }
    public class StringPopupAttributeDrawer : BasePopupAttributeDrawer<string>
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (this.dmValue == null)
                this.dmValue = string.Empty;
        }

        protected override string[] ProcessPopulation(object population)
        {
            return (population as string[]).Select(x => x.ToString()).ToArray();
        }

        protected override void SetValue(string value)
        {
            this.dmValue = value;
        }
    }
}