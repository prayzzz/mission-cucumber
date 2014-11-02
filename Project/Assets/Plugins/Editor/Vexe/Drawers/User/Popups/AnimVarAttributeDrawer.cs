using System.Text.RegularExpressions;

using Assets.Plugins.Editor.Vexe.Drawers.API.Base;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Popups;

using Fasterflect;

using UnityEngine;

using Vexe.Editor.Helpers;
using Vexe.Runtime.Extensions;

namespace Assets.Plugins.Editor.Vexe.Drawers.User.Popups
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
                if (this.mAnimator == null)
                {
                    string getter = this.attribute.GetAnimator;
                    if (getter.IsNullOrEmpty())
                    {
                        this.mAnimator = this.gameObject.GetComponent<Animator>();
                    }
                    else
                    {
                        this.mAnimator = this.targetType.GetMethod(getter, Flags.InstanceAnyVisibility)
                                                  .Invoke(this.rawTarget, null) as Animator;
                    }
                }
                return this.mAnimator;
            }
        }

        protected override void OnInitialized()
        {
            if (this.dmValue == null)
                this.dmValue = "";

            if (this.animator != null && this.animator.runtimeAnimatorController != null)
            {
                this.FetchVariables();
            }
        }

        private void FetchVariables()
        {
            this.variables = EditorHelper.GetAnimatorVariableNames(this.animator);
            if (this.variables.IsEmpty())
                this.variables = new[] { "N/A" };
            else
            {
                if (!this.attribute.AutoMatch.IsNullOrEmpty())
                {
                    string match = this.niceName.Remove(this.niceName.IndexOf(this.attribute.AutoMatch));
                    match = Regex.Replace(match, @"\s+", "");
                    if (this.variables.ContainsValue(match))
                        this.dmValue = match;
                }
                this.current = this.variables.IndexOfZeroIfNotFound(this.dmValue);
            }
        }

        public override void OnGUI()
        {
            if (this.animator == null || this.animator.runtimeAnimatorController == null)
            {
                this.dmValue = this.gui.TextField(this.niceName, this.dmValue);
            }
            else
            {
                if (this.variables.IsNullOrEmpty())
                {
                    this.FetchVariables();
                }

                var x = this.gui.Popup(this.niceName, this.current, this.variables);
                {
                    if (this.current != x || this.dmValue != this.variables[x])
                        this.dmValue = this.variables[this.current = x];
                }
            }
        }
    }
}