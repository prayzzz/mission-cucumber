using System.Collections.Generic;

using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Categories;
using Assets.Plugins.Vexe.Runtime.Types.Core;

using UnityEngine;

namespace Assets.Plugins.Vexe.Runtime.Types.Other
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

        public Dictionary<string, int> Ints                   { set { this._Ints = value;         } get { return this.GetLazyValue(ref this._Ints); } }
        public Dictionary<string, string> Strings             { set { this._Strings = value;      } get { return this.GetLazyValue(ref this._Strings); } }
        public Dictionary<string, float> Floats               { set { this._Floats = value;       } get { return this.GetLazyValue(ref this._Floats); } }
        public Dictionary<string, bool> Bools                 { set { this._Bools = value;        } get { return this.GetLazyValue(ref this._Bools); } }
        public Dictionary<string, Vector3> Vector3s           { set { this._Vector3s = value;     } get { return this.GetLazyValue(ref this._Vector3s); } }
        public Dictionary<string, Color> Colors               { set { this._Colors = value;       } get { return this.GetLazyValue(ref this._Colors); } }

        public Dictionary<string, List<int>> IntLists         { set { this._IntLists = value;     } get { return this.GetLazyValue(ref this._IntLists); } }
        public Dictionary<string, List<string>> StringLists   { set { this._StringLists = value;  } get { return this.GetLazyValue(ref this._StringLists); } }
        public Dictionary<string, List<float>> FloatLists     { set { this._FloatLists = value;   } get { return this.GetLazyValue(ref this._FloatLists); } }
        public Dictionary<string, List<bool>> BoolLists       { set { this._BoolLists = value;    } get { return this.GetLazyValue(ref this._BoolLists); } }
        public Dictionary<string, List<Vector3>> Vector3Lists { set { this._Vector3Lists = value; } get { return this.GetLazyValue(ref this._Vector3Lists); } }
        public Dictionary<string, List<Color>> ColorLists     { set { this._ColorLists = value;   } get { return this.GetLazyValue(ref this._ColorLists); } }

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