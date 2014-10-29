using System.Collections.Generic;
using UnityEngine;

namespace Vexe.Runtime.Types
{
	[DefineCategories("Plural", "Single")]
	public class BetterPrefs : BetterScriptableObject
	{
		private const string Plural = "Plural", Single = "Single";

		[Category(Single), Save] private Dictionary<string, int> _Ints;
		[Category(Single), Save] private Dictionary<string, string> _Strings;
		[Category(Single), Save] private Dictionary<string, float> _Floats;
		[Category(Single), Save] private Dictionary<string, bool> _Bools;
		[Category(Single), Save] private Dictionary<string, Vector3> _Vector3s;
		[Category(Single), Save] private Dictionary<string, Color> _Colors;

		[Category(Plural), Save] private Dictionary<string, List<int>> _IntLists;
		[Category(Plural), Save] private Dictionary<string, List<string>> _StringLists;
		[Category(Plural), Save] private Dictionary<string, List<float>> _FloatLists;
		[Category(Plural), Save] private Dictionary<string, List<bool>> _BoolLists;
		[Category(Plural), Save] private Dictionary<string, List<Vector3>> _Vector3Lists;
		[Category(Plural), Save] private Dictionary<string, List<Color>> _ColorLists;

		public Dictionary<string, int> Ints                   { set { _Ints = value;         } get { return GetLazyValue(ref _Ints); } }
		public Dictionary<string, string> Strings             { set { _Strings = value;      } get { return GetLazyValue(ref _Strings); } }
		public Dictionary<string, float> Floats               { set { _Floats = value;       } get { return GetLazyValue(ref _Floats); } }
		public Dictionary<string, bool> Bools                 { set { _Bools = value;        } get { return GetLazyValue(ref _Bools); } }
		public Dictionary<string, Vector3> Vector3s           { set { _Vector3s = value;     } get { return GetLazyValue(ref _Vector3s); } }
		public Dictionary<string, Color> Colors               { set { _Colors = value;       } get { return GetLazyValue(ref _Colors); } }

		public Dictionary<string, List<int>> IntLists         { set { _IntLists = value;     } get { return GetLazyValue(ref _IntLists); } }
		public Dictionary<string, List<string>> StringLists   { set { _StringLists = value;  } get { return GetLazyValue(ref _StringLists); } }
		public Dictionary<string, List<float>> FloatLists     { set { _FloatLists = value;   } get { return GetLazyValue(ref _FloatLists); } }
		public Dictionary<string, List<bool>> BoolLists       { set { _BoolLists = value;    } get { return GetLazyValue(ref _BoolLists); } }
		public Dictionary<string, List<Vector3>> Vector3Lists { set { _Vector3Lists = value; } get { return GetLazyValue(ref _Vector3Lists); } }
		public Dictionary<string, List<Color>> ColorLists     { set { _ColorLists = value;   } get { return GetLazyValue(ref _ColorLists); } }

		private Dictionary<string, T> GetLazyValue<T>(ref Dictionary<string, T> dictionary)
		{
			return dictionary ?? (dictionary = new Dictionary<string, T>());
		}
	}

#if UNITY_EDITOR
	public static class BetterPrefsMenus
	{
		[UnityEditor.MenuItem("Tools/Vexe/BetterPrefs/CreateAsset")]
		public static void CreateBetterPrefsAsset()
		{
			UnityEditor.AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<BetterPrefs>(), "Assets/Vexe/Runtime/ScriptableAssets/BetterPrefs.asset");
		}
	}
#endif
}