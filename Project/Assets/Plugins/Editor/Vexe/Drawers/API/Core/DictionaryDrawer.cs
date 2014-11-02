using System;
using System.Collections.Generic;
using System.Linq;

using Assets.Plugins.Editor.Vexe.Drawers.API.Base;
using Assets.Plugins.Editor.Vexe.GUI;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Others;

using Fasterflect;

using Smooth.Algebraics;

using UnityEngine;

using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

using Object = UnityEngine.Object;

namespace Assets.Plugins.Editor.Vexe.Drawers.API.Core
{
    [CoreDrawer]
    public class DictionaryDrawer<TKey, TValue> : ObjectDrawer<Dictionary<TKey, TValue>>
    {
        private readonly List<ElementInfo<TKey>> keyElements;
        private readonly List<ElementInfo<TValue>> valueElements;
        private readonly List<string> cachedKeyStrings, cachedValueStrings, cachedPairStrings;
        private readonly DataMember addKeyInfo, addValueInfo;
        private readonly AddInfo addInfo;
        private KVPList<TKey, TValue> dictionary;
        private bool perKeyDrawing;
        private bool perValueDrawing;
        private bool ignoreAddArea;
        private string dictionaryName;
        private string addKey;

        public bool Readonly { get; set; }

        public DictionaryDrawer()
        {
            this.keyElements        = new List<ElementInfo<TKey>>();
            this.valueElements      = new List<ElementInfo<TValue>>();
            this.cachedKeyStrings     = new List<string>();
            this.cachedValueStrings = new List<string>();
            this.cachedPairStrings  = new List<string>();
            this.addInfo            = new AddInfo();
            this.addKeyInfo         = new DataMember(typeof(AddInfo).Field("key", Flags.InstancePublic), this.addInfo);
            this.addValueInfo       = new DataMember(typeof(AddInfo).Field("value", Flags.InstancePublic), this.addInfo);
            dbgLog("Dictionary drawer Awaken");
        }

        protected override void OnInitialized()
        {
            this.perKeyDrawing   = this.attributes.AnyIs<PerKeyAttribute>();
            this.perValueDrawing = this.attributes.AnyIs<PerValueAttribute>();
            this.ignoreAddArea   = this.attributes.AnyIs<IgnoreAddArea>();
            this.Readonly            = this.attributes.AnyIs<ReadonlyAttribute>();
            this.dictionaryName  = this.niceName + " (" + this.dataMember.Type.GetFriendlyName() + ")";
            this.addKey          = this.id + "add";
            dbgLog("Dictionary drawer Initialized");
        }

        public class AddInfo
        {
            public TKey key;
            public TValue value;
        }

        public override void OnGUI()
        {
            this.foldout = this.gui.Foldout(this.dictionaryName, this.foldout, GUILayout.ExpandWidth(true));

            if (!this.foldout) return;

            this.Read();

            if (!this.Readonly)
            {
                this.gui._beginIndent();
                Profiler.BeginSample("DictionaryDrawer Header");
                {
                    var kStr = this.GetString(this.cachedKeyStrings, this.addInfo.key, 0);
                    var vStr = this.GetString(this.cachedValueStrings, this.addInfo.value, 0);
                    var pStr = this.GetPairString().Invoke(new Tuple<string, string>(kStr, vStr));
                    this.gui._beginH();
                    {
                        this.SetEntryFoldout(this.addKey, this.gui.Foldout(this.prefs.GetSafeBool(this.addKey)));

                        this.gui._beginWidth(70f);
                        {
                            this.gui.TextField("Add pair:", pStr);
                        }
                        this.gui._endWidth();

                        this.gui._beginState(this.dictionary.Count > 0);
                        {
                            if (this.gui.RemoveButton("Last dictionary pair"))
                                this.RemoveLastPair();
                        }
                        this.gui._endState();

                        if (this.gui.AddButton("pair", MiniButtonStyle.ModRight))
                            this.AddPair(this.addInfo.key, this.addInfo.value);
                    }
                    this.gui._endH();

                    if (this.GetEntryFoldout(this.addKey))
                    {
                        this.gui._beginIndent();
                        Profiler.BeginSample("DictionaryDrawer AddingPair");
                        {
                            if (this.gui.MemberField(this.addKeyInfo, this.attributes, this.unityTarget, this.id, this.ignoreAddArea || !this.perKeyDrawing))
                                this.UpdateString(this.cachedKeyStrings, this.addKeyInfo.Get(), 0);

                            if (this.gui.MemberField(this.addValueInfo, this.attributes, this.unityTarget, this.id, this.ignoreAddArea || !this.perValueDrawing))
                                this.UpdateString(this.cachedValueStrings, this.addValueInfo.Get(), 0);
                        }
                        Profiler.EndSample();
                        this.gui._endIndent();
                    }
                }
                Profiler.EndSample();
                this.gui._endIndent();
            }

            this.gui._beginIndent();
            Profiler.BeginSample("DictionaryDrawer Pairs");
            {
                for (int i = 0; i < this.dictionary.Count; i++)
                {
                    var dKey   = this.dictionary.Keys[i];
                    var dValue = this.dictionary.Values[i];

                    Profiler.BeginSample("DictionaryDrawer KVP assignments");
                    var strIdx          = this.Readonly ? i : i + 1;
                    var kStr         = this.GetString(this.cachedKeyStrings, dKey, strIdx);
                    var vStr         = this.GetString(this.cachedValueStrings, dValue, strIdx);
                    var pStr              = this.GetPairString().Invoke(new Tuple<string, string>(kStr, vStr));

                    if (this.Readonly)
                    {
                        this.gui.TextField(pStr);
                        continue;
                    }

                    var foldoutKey   = this.GetKeyForIndex().Invoke(i);
                    var entryFoldout = this.GetEntryFoldout(foldoutKey);

                    var x = this.gui.Foldout(pStr, entryFoldout);
                    if (entryFoldout != x)
                        this.SetEntryFoldout(foldoutKey, x);
                    Profiler.EndSample();
                    
                    if (!this.GetEntryFoldout(foldoutKey))
                        continue;

                    this.gui._beginIndent();
                    Profiler.BeginSample("DictionaryDrawer SinglePair");
                    {
                        if (this.gui.MemberField(
                            this.GetElement(this.keyElements, this.dictionary.Keys, i, this.GetKeyForKey().Invoke(i)),
                            this.rawTarget, this.unityTarget, this.id, !this.perKeyDrawing)
                        ) this.UpdateString(this.cachedKeyStrings, this.keyElements[i].Get(), strIdx);

                        if (this.gui.MemberField(
                            this.GetElement(this.valueElements, this.dictionary.Values, i, this.GetKeyForValue().Invoke(i)),
                            this.rawTarget, this.unityTarget, this.id, !this.perValueDrawing)
                        ) this.UpdateString(this.cachedValueStrings, this.valueElements[i].Get(), strIdx);
                    }
                    Profiler.EndSample();
                    this.gui._endIndent();
                }
            }
            Profiler.EndSample();
            this.gui._endIndent();

            this.Write();
        }

        private Func<Tuple<string, string>, string> _stringForPair;
        private Func<Tuple<string, string>, string> GetPairString()
        {
            return this._stringForPair ?? (this._stringForPair = new Func<Tuple<string, string>, string>(
                pair => string.Format("[{0}, {1}]", pair._1, pair._2)).Memoize());
        }

        private Func<int, string> _keyForKey;
        private Func<int, string> GetKeyForKey()
        {
            return this._keyForKey ?? (this._keyForKey = new Func<int, string>(index => this.GetKeyForIndex().Invoke(index) + "keys").Memoize());
        }
        private Func<int, string> _keyForValue;
        private Func<int, string> GetKeyForValue()
        {
            return this._keyForValue ?? (this._keyForValue = new Func<int, string>(index => this.GetKeyForIndex().Invoke(index) + "values").Memoize());
        }
        private Func<int, string> _keyForIndex;
        private Func<int, string> GetKeyForIndex()
        {
            return this._keyForIndex ?? (this._keyForIndex = new Func<int, string>(index => this.id + index).Memoize());
        }

        private void SetValueAt(int i, TValue value)
        {
            this.dictionary.SetValueAt(i, value);
        }

        private void SetKeyAt(int i, TKey key)
        {
            this.dictionary.SetKeyAt(i, key);
        }

        private ElementInfo<T> GetElement<T>(List<ElementInfo<T>> elements, List<T> source, int index, string id)
        {
            if (index >= elements.Count)
            {
                var element= new ElementInfo<T>(
                    id            : id,
                    attributes    : this.attributes.ToArray(),
                    name          : string.Empty,
                    elementType   : typeof(T),
                    declaringType : this.rawTarget.GetType(),
                    reflectedType : this.GetType()
                ) { List = source, Index = index };
                elements.Add(element);
                return element;
            }
            var e   = elements[index];
            e.List  = source;
            e.Index = index;
            return e;
        }

        private string GetString<T>(List<string> strings, T obj, int index)
        {
            if (index >= strings.Count)
            {
                strings.Add(this.GetObjectString(obj));
            }
            return strings[index];
        }

        private void UpdateString<T>(List<string> strings, T obj, int index)
        {
            strings[index] = this.GetObjectString(obj);
        }

        private string GetObjectString(object from)
        {
            if (from.IsObjectNull())
                return "null";
            var obj = from as Object;
            return (obj != null) ? obj.name : from.ToString();
        }

        private void RemoveLastPair()
        {
            this.dictionary.RemoveLast();
        }

        private void AddPair(TKey key, TValue value)
        {
            try
            {
                if (typeof(TKey) == typeof(string))
                {
                    var str = key as string;
                    if (str == null)
                        key = (TKey)(object)string.Empty;
                }
                this.dictionary.Add(key, value);
            }
            catch (ArgumentException e)
            {
                dbgLog(e.Message);
            }
        }

        private bool GetEntryFoldout(string keyString)
        {
            return this.prefs.GetSafeBool(keyString);
        }

        private void SetEntryFoldout(string keyString, bool value)
        {
            this.prefs.SetBool(keyString, value);
        }

        private void Read()
        {
            this.dictionary = this.dmValue == null ? new KVPList<TKey, TValue>() : this.dmValue.ToKVPList();
        }

        private void Write()
        {
            this.dmValue = this.dictionary.ToDictionary();
        }
    }
}