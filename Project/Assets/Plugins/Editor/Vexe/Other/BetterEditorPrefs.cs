using System;
using System.Collections.Generic;

using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Categories;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Others;
using Assets.Plugins.Vexe.Runtime.Types.Core;

using UnityEditor;

using UnityEngine;

using Vexe.Editor.Helpers;

namespace Assets.Plugins.Editor.Vexe.Other
{
    /// <summary>
    /// A prefs class used to store boolean values at dictionaries with string/int keys
    /// Could be instantiated (via CreateInstance) or treated globally (via Instance)
    /// If treated a globally, a ScriptableObject is created and stored as an asset at "*/ScriptableAssets/BetterPrefs.asset"
    /// </summary>
    [Serializable, BasicView]
    public class BetterEditorPrefs : BetterScriptableObject
    {
        private const string MenuPath = "Tools/Vexe/BetterPrefs";
        private static readonly string Path = EditorHelper.ScriptableAssetsPath + "/BetterEditorPrefs.asset";
        private static BetterEditorPrefs instance;

        [Save, Readonly] private Dictionary<string, bool> _bools;
        private Dictionary<string, bool> bools
        {
            get { return this._bools ?? (this._bools = new Dictionary<string, bool>()); }
        }
        [Save, Readonly] private Dictionary<string, int> _ints;
        private Dictionary<string, int> ints
        {
            get { return this._ints ?? (this._ints = new Dictionary<string, int>()); }
        }

        /// <summary>
        /// Returns the BetterPrefs singleton instance
        /// </summary>
        public static BetterEditorPrefs Instance
        {
            get { return EditorHelper.LazyLoadScriptableAsset<BetterEditorPrefs>(ref instance, Path, true); }
        }

        [Show] public void Clear()
        {
            this.bools.Clear();
            this.ints.Clear();
        }

        /// <summary>
        /// Creates and returns a new BetterPrefs instance
        /// </summary>
        public static BetterEditorPrefs CreateInstance()
        {
            return ScriptableObject.CreateInstance<BetterEditorPrefs>();
        }

        /// <summary>
        /// Sets the boolean dictionary at the specified key to the specified boolean value
        /// </summary>
        public void SetBool(string key, bool? value)
        {
            if (!value.HasValue)
                throw new ArgumentNullException("Nullable boolean must have a value");
            this.SetBool(key, value.Value);
        }

        /// <summary>
        /// Sets the boolean dictionary at the specified key to the specified boolean value
        /// </summary>
        public void SetBool(string key, bool value)
        {
            this.bools[key] = value;
            EditorUtility.SetDirty(this);
        }

        /// <summary>
        /// Returns the boolean value from the dictionary at the specified key
        /// </summary>
        public bool? GetBool(string key)
        {
            bool value;
            return this.bools.TryGetValue(key, out value) ? value : (bool?)null;
        }

        /// <summary>
        /// Returns the boolean value from the dictionary at the specified key
        /// If the boolean doesn't have a value (null) false is returned instead
        /// </summary>
        public bool GetSafeBool(string key)
        {
            var ret = this.GetBool(key);
            return ret.HasValue ? ret.Value : false;
        }

        /// <summary>
        /// Returns the boolean value from the dictionary at the specified key
        /// If the boolean doesn't have a value (null) false is returned instead
        /// </summary>
        public static bool sGetSafeBool(string key)
        {
            return Instance.GetSafeBool(key);
        }

        /// <summary>
        /// Sets the boolean dictionary at the specified key to the specified boolean value
        /// </summary>
        public static void sSetBool(string key, bool value)
        {
            Instance.SetBool(key, value);
        }

        /// <summary>
        /// Sets the intean dictionary at the specified key to the specified intean value
        /// </summary>
        public void SetInt(string key, int? value)
        {
            if (!value.HasValue)
                throw new ArgumentNullException("Nullable intean must have a value");
            this.SetInt(key, value.Value);
        }

        /// <summary>
        /// Sets the intean dictionary at the specified key to the specified intean value
        /// </summary>
        public void SetInt(string key, int value)
        {
            this.ints[key] = value;
            EditorUtility.SetDirty(this);
        }

        /// <summary>
        /// Returns the intean value from the dictionary at the specified key
        /// </summary>
        public int? GetInt(string key)
        {
            int value;
            return this.ints.TryGetValue(key, out value) ? value : (int?)null;
        }

        /// <summary>
        /// Returns the intean value from the dictionary at the specified key
        /// If the intean doesn't have a value (null) false is returned instead
        /// </summary>
        public int GetSafeInt(string key)
        {
            var ret = this.GetInt(key);
            return ret.HasValue ? ret.Value : 0;
        }

        /// <summary>
        /// Returns the intean value from the dictionary at the specified key
        /// If the intean doesn't have a value (null) false is returned instead
        /// </summary>
        public static int sGetSafeInt(string key)
        {
            return Instance.GetSafeInt(key);
        }

        /// <summary>
        /// Sets the intean dictionary at the specified key to the specified intean value
        /// </summary>
        public static void sSetInt(string key, int value)
        {
            Instance.SetInt(key, value);
        }
    }
}