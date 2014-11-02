using System;

using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Categories;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Requirements;
using Assets.Plugins.Vexe.Runtime.Types.Core;

using UnityEngine;

namespace Assets.Plugins.Vexe.Runtime.Types.Other
{
    /// <summary>
    /// An Animator wrapper that provides easy access to the underlying animator variables and functions
    /// </summary>
    [MinimalView, RequireComponent(typeof(Animator))]
    public class BetterAnimator : BetterBehaviour
    {
        // the first RequireComponent assures that the component is going to be available/attached
        // this requirement will make sure that the added animator is assigned to this field
        // so we don't have to do the assignment in OnAwake etc
        [Serialize, RequiredFromThis]
        private Animator animator;

        [Save, Hide] private IndexedBoolean bools;
        [Save, Hide] private IndexedFloat floats;
        [Save, Hide] private IndexedInteger ints;

        /// <summary>
        /// A quick access to the animator's integer variables
        /// ex:
        /// anim.Ints["Counter"]++;
        /// ...
        /// if (anim.Ints["SomeInt"] == someValue)
        /// ...
        /// </summary>
        public IndexedInteger Ints { get { return this.ints; } }

        /// <summary>
        /// A quick access to the animator's boolean variables
        /// ex:
        /// anim.Bools["IsDead"] = health <= 0;
        /// ...
        /// if (anim.Bools["IsShooting"])
        /// ...
        /// </summary>
        public IndexedBoolean Bools { get { return this.bools; } }

        /// <summary>
        /// A quick access to the animator's float variables
        /// ex:
        /// anim.Floats["Speed"] = 10;
        /// ...
        /// if (anim.Floats["Direction"] > 0)
        /// ...
        /// </summary>
        public IndexedFloat Floats { get { return this.floats; } }

        /// <summary>
        /// The underlying animator's animation playback speed
        /// </summary>
        public float speed
        {
            get { return this.animator.speed; }
            set { this.animator.speed = value; }
        }

        private void Awake()
        {
            if (this.bools.IsNull()) this.bools   = new IndexedBoolean() { animator = this.animator };
            if (this.ints.IsNull()) this.ints     = new IndexedInteger() { animator = this.animator };
            if (this.floats.IsNull()) this.floats = new IndexedFloat()   { animator = this.animator };
        }

        /// <summary>
        /// Sets the trigger parameter to active
        /// </summary>
        public void SetTrigger(string name)
        {
            this.animator.SetTrigger(name);
        }

        /// <summary>
        /// Sets the trigger parameter to active
        /// </summary>
        public void SetTrigger(int id)
        {
            this.animator.SetTrigger(id);
        }

        /// <summary>
        /// Resets the trigger parameter to false
        /// </summary>
        public void ResetTrigger(string id)
        {
            this.animator.ResetTrigger(this.name);
        }

        /// <summary>
        /// Resets the trigger parameter to false
        /// </summary>
        public void ResetTrigger(int id)
        {
            this.animator.ResetTrigger(id);
        }

        /// <summary>
        /// Is the animator in the state specified by 'stateName' and 'layerIndex'?
        /// </summary>
        public bool IsInState(int layerIndex, string stateName)
        {
            return this.animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName);
        }

        /// <summary>
        /// Is the animator in the state specified by 'stateHash' and 'layerIndex'?
        /// </summary>
        public bool IsInState(int layerIndex, int stateHash)
        {
            return this.animator.GetCurrentAnimatorStateInfo(layerIndex).nameHash == stateHash;
        }

        /// <summary>
        /// Is the animator in the state specified by 'stateName' and a layer index of 0?
        /// </summary>
        public bool IsInState(string stateName)
        {
            return this.IsInState(0, stateName);
        }

        /// <summary>
        /// Is the animator in the state specified by 'stateHash' and a layer index of 0?
        /// </summary>
        public bool IsInState(int stateHash)
        {
            return this.IsInState(0, stateHash);
        }

        [Serializable]
        public abstract class IndexedMecanimVariable<T>
        {
            public Animator animator;
            public abstract T this[string key] { get; set; }
        }

        [Serializable]
        public class IndexedBoolean : IndexedMecanimVariable<bool>
        {
            public override bool this[string key]
            {
                get { return this.animator.GetBool(key); }
                set { this.animator.SetBool(key, value); }
            }
        }

        [Serializable]
        public class IndexedInteger : IndexedMecanimVariable<int>
        {
            public override int this[string key]
            {
                get { return this.animator.GetInteger(key); }
                set { this.animator.SetInteger(key, value); }
            }
        }

        [Serializable]
        public class IndexedFloat : IndexedMecanimVariable<float>
        {
            public override float this[string key]
            {
                get { return this.animator.GetFloat(key); }
                set { this.animator.SetFloat(key, value); }
            }
        }
    }

    public static class IndexedMecanimVariablesExtensions
    {
        public static bool IsNull<T>(this BetterAnimator.IndexedMecanimVariable<T> variable)
        {
            return variable == null || variable.animator == null || variable.animator.Equals(null);
        }
    }
}