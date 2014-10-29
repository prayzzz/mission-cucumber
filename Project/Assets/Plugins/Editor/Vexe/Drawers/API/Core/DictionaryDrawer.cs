using System;
using System.Collections.Generic;
using System.Linq;
using Fasterflect;
using Smooth.Algebraics;
using UnityEngine;
using Vexe.Editor.Framework.GUIs;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;
using UnityObject = UnityEngine.Object;

namespace Vexe.Editor.Framework.Drawers
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
			keyElements        = new List<ElementInfo<TKey>>();
			valueElements      = new List<ElementInfo<TValue>>();
			cachedKeyStrings	 = new List<string>();
			cachedValueStrings = new List<string>();
			cachedPairStrings  = new List<string>();
			addInfo            = new AddInfo();
			addKeyInfo         = new DataMember(typeof(AddInfo).Field("key", Flags.InstancePublic), addInfo);
			addValueInfo       = new DataMember(typeof(AddInfo).Field("value", Flags.InstancePublic), addInfo);
			dbgLog("Dictionary drawer Awaken");
		}

		protected override void OnInitialized()
		{
			perKeyDrawing   = attributes.AnyIs<PerKeyAttribute>();
			perValueDrawing = attributes.AnyIs<PerValueAttribute>();
			ignoreAddArea   = attributes.AnyIs<IgnoreAddArea>();
			Readonly		    = attributes.AnyIs<ReadonlyAttribute>();
			dictionaryName  = niceName + " (" + dataMember.Type.GetFriendlyName() + ")";
			addKey          = id + "add";
			dbgLog("Dictionary drawer Initialized");
		}

		public class AddInfo
		{
			public TKey key;
			public TValue value;
		}

		public override void OnGUI()
		{
			foldout = gui.Foldout(dictionaryName, foldout, GUILayout.ExpandWidth(true));

			if (!foldout) return;

			Read();

			if (!Readonly)
			{
				gui._beginIndent();
				Profiler.BeginSample("DictionaryDrawer Header");
				{
					var kStr = GetString(cachedKeyStrings, addInfo.key, 0);
					var vStr = GetString(cachedValueStrings, addInfo.value, 0);
					var pStr = GetPairString().Invoke(new Tuple<string, string>(kStr, vStr));
					gui._beginH();
					{
						SetEntryFoldout(addKey, gui.Foldout(prefs.GetSafeBool(addKey)));

						gui._beginWidth(70f);
						{
							gui.TextField("Add pair:", pStr);
						}
						gui._endWidth();

						gui._beginState(dictionary.Count > 0);
						{
							if (gui.RemoveButton("Last dictionary pair"))
								RemoveLastPair();
						}
						gui._endState();

						if (gui.AddButton("pair", MiniButtonStyle.ModRight))
							AddPair(addInfo.key, addInfo.value);
					}
					gui._endH();

					if (GetEntryFoldout(addKey))
					{
						gui._beginIndent();
						Profiler.BeginSample("DictionaryDrawer AddingPair");
						{
							if (gui.MemberField(addKeyInfo, attributes, unityTarget, id, ignoreAddArea || !perKeyDrawing))
								UpdateString(cachedKeyStrings, addKeyInfo.Get(), 0);

							if (gui.MemberField(addValueInfo, attributes, unityTarget, id, ignoreAddArea || !perValueDrawing))
								UpdateString(cachedValueStrings, addValueInfo.Get(), 0);
						}
						Profiler.EndSample();
						gui._endIndent();
					}
				}
				Profiler.EndSample();
				gui._endIndent();
			}

			gui._beginIndent();
			Profiler.BeginSample("DictionaryDrawer Pairs");
			{
				for (int i = 0; i < dictionary.Count; i++)
				{
					var dKey   = dictionary.Keys[i];
					var dValue = dictionary.Values[i];

					Profiler.BeginSample("DictionaryDrawer KVP assignments");
					var strIdx		  = Readonly ? i : i + 1;
					var kStr         = GetString(cachedKeyStrings, dKey, strIdx);
					var vStr         = GetString(cachedValueStrings, dValue, strIdx);
					var pStr			  = GetPairString().Invoke(new Tuple<string, string>(kStr, vStr));

					if (Readonly)
					{
						gui.TextField(pStr);
						continue;
					}

					var foldoutKey   = GetKeyForIndex().Invoke(i);
					var entryFoldout = GetEntryFoldout(foldoutKey);

					var x = gui.Foldout(pStr, entryFoldout);
					if (entryFoldout != x)
						SetEntryFoldout(foldoutKey, x);
					Profiler.EndSample();
					
					if (!GetEntryFoldout(foldoutKey))
						continue;

					gui._beginIndent();
					Profiler.BeginSample("DictionaryDrawer SinglePair");
					{
						if (gui.MemberField(
							GetElement(keyElements, dictionary.Keys, i, GetKeyForKey().Invoke(i)),
							rawTarget, unityTarget, id, !perKeyDrawing)
						) UpdateString(cachedKeyStrings, keyElements[i].Get(), strIdx);

						if (gui.MemberField(
							GetElement(valueElements, dictionary.Values, i, GetKeyForValue().Invoke(i)),
							rawTarget, unityTarget, id, !perValueDrawing)
						) UpdateString(cachedValueStrings, valueElements[i].Get(), strIdx);
					}
					Profiler.EndSample();
					gui._endIndent();
				}
			}
			Profiler.EndSample();
			gui._endIndent();

			Write();
		}

		private Func<Tuple<string, string>, string> _stringForPair;
		private Func<Tuple<string, string>, string> GetPairString()
		{
			return _stringForPair ?? (_stringForPair = new Func<Tuple<string, string>, string>(
				pair => string.Format("[{0}, {1}]", pair._1, pair._2)).Memoize());
		}

		private Func<int, string> _keyForKey;
		private Func<int, string> GetKeyForKey()
		{
			return _keyForKey ?? (_keyForKey = new Func<int, string>(index => GetKeyForIndex().Invoke(index) + "keys").Memoize());
		}
		private Func<int, string> _keyForValue;
		private Func<int, string> GetKeyForValue()
		{
			return _keyForValue ?? (_keyForValue = new Func<int, string>(index => GetKeyForIndex().Invoke(index) + "values").Memoize());
		}
		private Func<int, string> _keyForIndex;
		private Func<int, string> GetKeyForIndex()
		{
			return _keyForIndex ?? (_keyForIndex = new Func<int, string>(index => id + index).Memoize());
		}

		private void SetValueAt(int i, TValue value)
		{
			dictionary.SetValueAt(i, value);
		}

		private void SetKeyAt(int i, TKey key)
		{
			dictionary.SetKeyAt(i, key);
		}

		private ElementInfo<T> GetElement<T>(List<ElementInfo<T>> elements, List<T> source, int index, string id)
		{
			if (index >= elements.Count)
			{
				var element= new ElementInfo<T>(
					@id            : id,
					@attributes    : attributes.ToArray(),
					@name          : string.Empty,
					@elementType   : typeof(T),
					@declaringType : rawTarget.GetType(),
					@reflectedType : GetType()
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
				strings.Add(GetObjectString(obj));
			}
			return strings[index];
		}

		private void UpdateString<T>(List<string> strings, T obj, int index)
		{
			strings[index] = GetObjectString(obj);
		}

		private string GetObjectString(object from)
		{
			if (from.IsObjectNull())
				return "null";
			var obj = from as UnityObject;
			return (obj != null) ? obj.name : from.ToString();
		}

		private void RemoveLastPair()
		{
			dictionary.RemoveLast();
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
				dictionary.Add(key, value);
			}
			catch (ArgumentException e)
			{
				dbgLog(e.Message);
			}
		}

		private bool GetEntryFoldout(string keyString)
		{
			return prefs.GetSafeBool(keyString);
		}

		private void SetEntryFoldout(string keyString, bool value)
		{
			prefs.SetBool(keyString, value);
		}

		private void Read()
		{
			dictionary = dmValue == null ? new KVPList<TKey, TValue>() : dmValue.ToKVPList();
		}

		private void Write()
		{
			dmValue = dictionary.ToDictionary();
		}
	}
}