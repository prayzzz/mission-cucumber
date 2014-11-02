using System;

using Assets.Plugins.Vexe.Runtime.Types.Core;

using UnityEditor;

using UnityEngine;

namespace Assets.Plugins.Editor.Vexe.CustomEditors
{
    [CustomEditor(typeof(BetterBehaviour), true)]
    public class BetterBehaviourEditor : BetterEditor
    {
        protected override bool dbg
        {
            get { return this.GetCustomTypedTarget<BetterBehaviour>().dbg; }
            set { this.GetCustomTypedTarget<BetterBehaviour>().dbg = value; ; }
        }

        protected override void OnAwakeAssertion()
        {
            if (this.target is MonoBehaviour && !(this.target is BetterBehaviour))
            {
                throw new InvalidOperationException("target is a MonoBehaviour but not a BetterBehaviour. Please inherit BetterBehaviour");
            }

            if ((this.target as BetterBehaviour) == null)
            {
                Debug.LogError(string.Concat(new[] {
                                "Casting target object to BetterBehaviour failed! Something's wrong. ",
                                "Maybe you switched back and inherited MonoBehaviour instead of BetterBehaviour ",
                                "and you still had your gameObject selected? ",
                                "If that's the case then the BetterBehaviourEditor is still there in memory ",
                                "and so this could be resolved by reselcting your gameObject. ",
                                "Destroying this BetterBehaviourEditor instance anyway..."
                            }));
                DestroyImmediate(this);
                return;
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}