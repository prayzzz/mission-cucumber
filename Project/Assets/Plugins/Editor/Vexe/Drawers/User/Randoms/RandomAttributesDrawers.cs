using Assets.Plugins.Editor.Vexe.Drawers.API.Base;
using Assets.Plugins.Editor.Vexe.GUI;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Randoms;

using UnityEngine;

namespace Assets.Plugins.Editor.Vexe.Drawers.User.Randoms
{
    public abstract class RandomAttributeDrawer<T> : CompositeDrawer<T, RandAttribute>
    {
        protected override void OnInitialized()
        {
            // Randomize once when we're initialized
            if (!this.foldout)
            {
                this.foldout = true;
                this.Randomize();
            }
        }

        public override void OnRightGUI()
        {
            if (this.gui.MiniButton("R", "Randomize", MiniButtonStyle.Right))
                this.Randomize();
        }

        protected abstract void Randomize();
    }

    public class RandomfloatAttributeDrawer : RandomAttributeDrawer<float>
    {
        protected override void Randomize()
        {
            this.dmValue = Random.Range(this.attribute.floatMin, this.attribute.floatMax);
        }
    }

    public class RandomIntAttributeDrawer : RandomAttributeDrawer<int>
    {
        protected override void Randomize()
        {
            this.dmValue = Random.Range(this.attribute.intMin, this.attribute.intMax);
        }
    }
}