using System;

using Assets.Plugins.Vexe.Runtime.Types.Core;

using UnityEditor;

using UnityEngine;

namespace Assets.Plugins.Editor.Vexe.CustomEditors
{
    [CustomEditor(typeof(BetterScriptableObject), true)]
    public class BetterScriptableObjectEditor : BetterEditor
    {
        protected override bool dbg
        {
            get { return this.GetCustomTypedTarget<BetterScriptableObject>().dbg; }
            set { this.GetCustomTypedTarget<BetterScriptableObject>().dbg = value; ; }
        }

        protected override void OnAwakeAssertion()
        {
            if (this.target is ScriptableObject && !(this.target is BetterScriptableObject))
            {
                throw new InvalidOperationException("target is a ScriptableObject but not a BetterScriptableObject. Please inherit BetterScriptableObject");
            }

            if ((this.target as BetterScriptableObject) == null)
            {
                Debug.LogError(string.Concat(new[] {
                                "Casting target object to BetterScriptableObject failed! Something's wrong. ",
                                "Maybe you switched back and inherited ScriptableObject instead of BetterScriptableObject ",
                                "and you still had your gameObject selected? ",
                                "If that's the case then the BetterScriptableObjectEditor is still there in memory ",
                                "and so this could be resolved by reselcting your gameObject. ",
                                "Destroying this BetterScriptableObjectEditor instance anyway..."
                            }));
                DestroyImmediate(this);
                return;
            }
        }
    }
}