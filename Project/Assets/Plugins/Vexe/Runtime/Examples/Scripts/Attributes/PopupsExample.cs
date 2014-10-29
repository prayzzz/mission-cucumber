using System.Linq;

namespace Vexe.Runtime.Types.Examples
{
	public class PopupsExample : BetterBehaviour
	{
		[Popup("String1", "String2", "String3")]
		public string strings;

		// populate from the method GetFactors - which should return a string[]
		// (access modifier on the method doesn't matter)
		// also use a filter to quickly select values
		[Popup("GetFactors")]
		public string Factor { get; set; }

		[Popup(1.5f, 1.2f, 2.5f, 40.5f)]
		public float Floats { get; set; }

		[Popup(1, 2, 3)]
		public int ints;

		[Popup("GetCounts")]
		public int count;

		// PerItem indicates that the attributes are applied per element, and not on the array
		// in this case, Tags and OnChanged will be applied on each element
		// if the value of any element changes, LogFactor is calling with the new value
		[PerItem, Tags, OnChanged("Log")]
		public string[] EnemyTags { get; set; }

		[Tags]
		public string playerTag;

		private string[] GetFactors()
		{
			return new[] { "Ork", "Human", "Undead", "Elves" };
		}

		private int[] GetCounts()
		{
			return GetFactors().Select(f => f.Length).ToArray();
		}
	}
}