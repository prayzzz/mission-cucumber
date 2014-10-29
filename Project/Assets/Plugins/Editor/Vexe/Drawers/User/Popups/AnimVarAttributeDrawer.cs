using System.Text.RegularExpressions;
using Fasterflect;
using UnityEngine;
using Vexe.Editor.Helpers;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Vexe.Editor.Framework.Drawers
{
	public class AnimatorVariableAttributeDrawer : AttributeDrawer<string, AnimVarAttribute>
	{
		private string[] variables;
		private Animator mAnimator;
		private int current;

		private Animator animator
		{
			get
			{
				if (mAnimator == null)
				{
					string getter = attribute.GetAnimator;
					if (getter.IsNullOrEmpty())
					{
						mAnimator = gameObject.GetComponent<Animator>();
					}
					else
					{
						mAnimator = targetType.GetMethod(getter, Flags.InstanceAnyVisibility)
										          .Invoke(rawTarget, null) as Animator;
					}
				}
				return mAnimator;
			}
		}

		protected override void OnInitialized()
		{
			if (dmValue == null)
				dmValue = "";

			if (animator != null && animator.runtimeAnimatorController != null)
			{
				FetchVariables();
			}
		}

		private void FetchVariables()
		{
			variables = EditorHelper.GetAnimatorVariableNames(animator);
			if (variables.IsEmpty())
				variables = new[] { "N/A" };
			else
			{
				if (!attribute.AutoMatch.IsNullOrEmpty())
				{
					string match = niceName.Remove(niceName.IndexOf(attribute.AutoMatch));
					match = Regex.Replace(match, @"\s+", "");
					if (variables.ContainsValue(match))
						dmValue = match;
				}
				current = variables.IndexOfZeroIfNotFound(dmValue);
			}
		}

		public override void OnGUI()
		{
			if (animator == null || animator.runtimeAnimatorController == null)
			{
				dmValue = gui.TextField(niceName, dmValue);
			}
			else
			{
				if (variables.IsNullOrEmpty())
				{
					FetchVariables();
				}

				var x = gui.Popup(niceName, current, variables);
				{
					if (current != x || dmValue != variables[x])
						dmValue = variables[current = x];
				}
			}
		}
	}
}