using System;
using UnityEngine;

namespace Vexe.Runtime.Types.Examples
{
	[BasicView]
	public class AbstractsExample : BetterBehaviour
	{
		[Save] // or Serialize, or SerializerField
		private BaseStrategy strategy;

		[Show]
		private void Perform()
		{
			strategy.Perform();
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