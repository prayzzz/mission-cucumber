using System;

using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Categories;
using Assets.Plugins.Vexe.Runtime.Types.Core;

using UnityEngine;

namespace Assets.Plugins.Vexe.Runtime.Examples.Scripts.Serialization
{
    [BasicView]
    public class AbstractsExample : BetterBehaviour
    {
        [Save] // or Serialize, or SerializerField
        private BaseStrategy strategy;

        [Show]
        private void Perform()
        {
            this.strategy.Perform();
        }

        [Serializable]
        public abstract class BaseStrategy
        {
            public abstract void Perform();
        }

        [Serializable]
        public class Flank : BaseStrategy
        {
            [SerializeField] private int x;

            public override void Perform()
            {
                Log("Flanking");
            }
        }

        [Serializable]
        public class Sweep : BaseStrategy
        {
            [Save] private int y;

            public override void Perform()
            {
                Log("Sweeping");
            }
        }
    }
}