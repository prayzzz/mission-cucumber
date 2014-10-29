using Vexe.Editor.Framework.GUIs;
using Vexe.Runtime.Types;
using Random = UnityEngine.Random;

namespace Vexe.Editor.Framework.Drawers
{
	public abstract class RandomAttributeDrawer<T> : CompositeDrawer<T, RandAttribute>
	{
		protected override void OnInitialized()
		{
			// Randomize once when we're initialized
			if (!foldout)
			{
				foldout = true;
				Randomize();
			}
		}

		public override void OnRightGUI()
		{
			if (gui.MiniButton("R", "Randomize", MiniButtonStyle.Right))
				Randomize();
		}

		protected abstract void Randomize();
	}

	public class RandomfloatAttributeDrawer : RandomAttributeDrawer<float>
	{
		protected override void Randomize()
		{
			dmValue = Random.Range(attribute.floatMin, attribute.floatMax);
		}
	}

	public class RandomIntAttributeDrawer : RandomAttributeDrawer<int>
	{
		protected override void Randomize()
		{
			dmValue = Random.Range(attribute.intMin, attribute.intMax);
		}
	}
}