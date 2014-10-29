using System.Linq;
using System.Reflection;
using Fasterflect;
using Vexe.Runtime.Exceptions;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Vexe.Editor.Framework.Drawers
{
	public class OnChangedAttributeDrawer<T> : CompositeDrawer<T, OnChangedAttribute>
	{
		private MethodInvoker onChanged;
		private MemberSetter setter;
		private T previous;

		protected override void OnInitialized()
		{
			string call = attribute.Call;
			string set  = attribute.Set;

			if (!set.IsNullOrEmpty())
			{
				try
				{
					setter = targetType.DelegateForSetFieldValue(set);
				}
				catch
				{
					try
					{
						setter = targetType.DelegateForSetPropertyValue(set);
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
					onChanged = (from method in targetType.GetMethods(Flags.StaticInstanceAnyVisibility | BindingFlags.FlattenHierarchy)
									 where method.Name == call
									 where method.ReturnType == typeof(void)
									 let args = method.GetParameters()
									 where args.Length == 1
									 where args[0].ParameterType.IsAssignableFrom(memberType)
									 select method).FirstOrDefault().DelegateForCallMethod();
				}
				catch
				{
					throw new MemberNotFoundException(string.Format("Couldn't find an appropriate method to call with the name: {0} on target {1} when apply OnChanged on member {2}", call, rawTarget, memberInfo.Name));
				}
			}

			previous = dmValue;
		}

		public override void OnLowerGUI()
		{
			var current = dmValue;
			if (!current.GenericEqual(previous))
			{
				previous = current;
				onChanged.SafeInvoke(rawTarget, current);
				setter.SafeInvoke(rawTarget, current);
			}
		}
	}
}