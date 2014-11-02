using System.Linq;
using System.Reflection;

using Assets.Plugins.Editor.Vexe.Drawers.API.Base;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Others;

using Fasterflect;

using Vexe.Runtime.Exceptions;
using Vexe.Runtime.Extensions;

namespace Assets.Plugins.Editor.Vexe.Drawers.User.Others
{
    public class OnChangedAttributeDrawer<T> : CompositeDrawer<T, OnChangedAttribute>
    {
        private MethodInvoker onChanged;
        private MemberSetter setter;
        private T previous;

        protected override void OnInitialized()
        {
            string call = this.attribute.Call;
            string set  = this.attribute.Set;

            if (!set.IsNullOrEmpty())
            {
                try
                {
                    this.setter = this.targetType.DelegateForSetFieldValue(set);
                }
                catch
                {
                    try
                    {
                        this.setter = this.targetType.DelegateForSetPropertyValue(set);
                    }
                    catch
                    {
                        throw new MemberNotFoundException("Failed to get a field or property to set with the name: " + set);
                    }
                }
            }

            if (!call.IsNullOrEmpty())
            {
                try
                {
                    this.onChanged = (from method in this.targetType.GetMethods(Flags.StaticInstanceAnyVisibility | BindingFlags.FlattenHierarchy)
                                     where method.Name == call
                                     where method.ReturnType == typeof(void)
                                     let args = method.GetParameters()
                                     where args.Length == 1
                                     where args[0].ParameterType.IsAssignableFrom(this.memberType)
                                     select method).FirstOrDefault().DelegateForCallMethod();
                }
                catch
                {
                    throw new MemberNotFoundException(string.Format("Couldn't find an appropriate method to call with the name: {0} on target {1} when apply OnChanged on member {2}", call, this.rawTarget, this.memberInfo.Name));
                }
            }

            this.previous = this.dmValue;
        }

        public override void OnLowerGUI()
        {
            var current = this.dmValue;
            if (!current.GenericEqual(this.previous))
            {
                this.previous = current;
                this.onChanged.SafeInvoke(this.rawTarget, current);
                this.setter.SafeInvoke(this.rawTarget, current);
            }
        }
    }
}