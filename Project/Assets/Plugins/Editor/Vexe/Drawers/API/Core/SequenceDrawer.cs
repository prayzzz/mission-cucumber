#define PROFILE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Vexe.Editor.Framework.GUIs;
using Vexe.Editor.Helpers;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;
using UnityObject = UnityEngine.Object;

namespace Vexe.Editor.Framework.Drawers
{
	[CoreDrawer]
	public abstract class SequenceDrawer<TSequence, TElement> : ObjectDrawer<TSequence> where TSequence : IList<TElement>
	{
		private readonly Type elementType;
		private string sequenceName;
		private string advancedKey;
		private List<ElementInfo<TElement>> elements;
		private DisplayOptions options;
		private bool perItemDrawing;
		private int newSize;
		private bool shouldDrawAddingArea;

		private bool IsAdvancedChecked
		{
			get { return prefs.GetSafeBool(advancedKey); }
			set { prefs.SetBool(advancedKey, value); }
		}

		protected abstract TSequence GetNew();

		public SequenceDrawer()
		{
			elementType = typeof(TElement);
			elements    = new List<ElementInfo<TElement>>();
		}

		protected override void OnInitialized()
		{
			var seqOptions       = attributes.GetAttribute<SeqAttribute>();
			options              = new DisplayOptions(seqOptions != null ? seqOptions.options : SeqOpt.None);
			options.Advanced     = options.Advanced || attributes.AnyIs<AdvancedAttribute>();
			options.Readonly     = options.Readonly || attributes.AnyIs<ReadonlyAttribute>();

			// Sequence name
			{ 
				var builder = new StringBuilder();
				builder.Append(niceName);
				builder.Append(" (");
				builder.Append(memberType.GetFriendlyName());
				builder.Append(")");
				if (options.Readonly) 
					builder.Append(" (Readonly)");
				sequenceName = builder.ToString();
			}

			advancedKey          = id + sequenceName + "advanced";
			perItemDrawing       = attributes.AnyIs<PerItemAttribute>();
			shouldDrawAddingArea = !options.Readonly && elementType.IsA<UnityObject>();

			if (dmValue == null)
				dmValue = GetNew();
		}

		public override void OnGUI()
		{
			bool showAdvanced = options.Advanced && !options.Readonly;

			DrawHeader(
				@showAdd           : !options.Readonly,
				@add               : AddValue,
				@remove            : RemoveLast,
				@showAdvanedToggle : showAdvanced
			);

			if (!foldout) return;

			if (dmValue.IsEmpty())
			{
				gui.HelpBox("Sequence is empty");
				return;
			}

			gui._beginV(options.GuiBox ? GUI.skin.box : GUIStyle.none);
			{
				if (IsAdvancedChecked)
					DrawAdvancedArea();

				gui._beginIndent(options.GuiBox ? GUI.skin.box : GUIStyle.none);
				{
#if PROFILE
					Profiler.BeginSample("Sequence Elements");
#endif
					for (int i= 0; i < dmValue.Count; i++)
					{
						var current = dmValue[i];
						DrawField(
							@index        : i,
							@value        : current,
							@showLineNums : options.LineNumbers,
							@showInspect  : IsAdvancedChecked,
							@showMove     : IsAdvancedChecked && showAdvanced,
							@moveDown     : MoveElementDown,
							@moveUp       : MoveElementUp,
							@showRemove   : !options.Readonly && options.PerItemRemove,
							@remove       : RemoveAt
						);
					}
#if PROFILE
					Profiler.EndSample();
#endif
				}
				gui._endIndent();
			}
			gui._endV();

			if (shouldDrawAddingArea)
				DrawAddingArea();
		}

		// Drawings
		#region

		private void DrawAdvancedArea()
		{
			gui._beginIndent(GUI.skin.box);
			{
				gui._beginH();
				{
					newSize = gui.IntField("New size", newSize);
					if (gui.MiniButton("c", "Commit", MiniButtonStyle.ModRight))
					{
						if (newSize != dmValue.Count)
							dmValue.AdjustSize(newSize, RemoveAt, AddValue);
					}
				}
				gui._endH();

				gui._beginH();
				{
					gui.Label("Commands");
					if (gui.MiniButton("Shuffle", "Shuffle list (randomize the order of the list's elements", (GUILayoutOption[])null))
						Shuffle();
					if (gui.MoveDownButton())
						dmValue.Shift(true);
					if (gui.MoveUpButton())
						dmValue.Shift(false);
					Action<Predicate<TElement>> clearSection = predicate =>
					{
						for (int i = dmValue.Count - 1; i > -1; i--)
							if (predicate(dmValue[i]))
								RemoveAt(i);
					};

					if (!elementType.IsValueType && gui.MiniButton("N", "Filter nulls"))
						 clearSection(e => e == null);
					if (gui.ClearButton("elements", MiniButtonStyle.ModRight))
						Clear();
				}
				gui._endH();
			}
			gui._endIndent();
		}

		private void DrawAddingArea()
		{
			Action<UnityObject> addOnDrop = obj =>
			{
				var go = obj as GameObject;
				object value = go != null ? go.GetComponent(elementType) : obj;
				AddValue((TElement)value);
			};

			gui._beginIndent();
			{
				gui.DragDropArea<UnityObject>(
					@label: "+",
					@labelSize: 14,
					@style: EditorStyles.toolbarButton,
					@canSetVisualModeToCopy: dragObjects => dragObjects.All(o =>
					{
						var go = o as GameObject;
						return go == null ? o.GetType().IsA(elementType) : go.GetComponent(elementType) != null;
					}),
					@cursor: MouseCursor.Link,
					@onDrop: addOnDrop,
					@onMouseUp: () => SelectionWindow.Show(new Tab<UnityObject>(
						getValues: () => UnityObject.FindObjectsOfType(elementType),
						@getCurrent: () => null,
						@setTarget: item =>
						{
							AddValue((TElement)(object)item);
						},
						getValueName: value => value.name,
						@title: elementType.Name + "s")),
					@preSpace: 2f,
					@postSpace: 35f,
					@height: .5f
				);
			}
			gui._endIndent();
		}

		private Func<int, string> _keyForIndex;
		private Func<int, string> GetKeyForIndex()
		{
			return _keyForIndex ?? (_keyForIndex = new Func<int, string>(index => id + index).Memoize());
		}

		private void DrawField(int index, TElement value,
			bool showLineNums, bool showInspect,
			bool showMove, Action<int> moveDown, Action<int> moveUp,
			bool showRemove, Action<int> remove)
		{
			gui._beginH();
			{
				if (showLineNums)
					gui.NumericLabel(index);

				var previous = value;
				gui._beginChange();
				{
					gui._beginV();
					{
						gui.MemberField(GetElement(index), rawTarget, unityTarget, GetKeyForIndex().Invoke(index), !perItemDrawing);
					}
					gui._endV();
				}
				if (gui._endChange())
				{
					if (options.Readonly)
						dmValue[index] = previous;
				}

				if (showInspect)
				{
					var c = value as Component;
					var go = c == null ? value as GameObject : c.gameObject;
					if (go != null)
						gui.InspectButton(go);
				}

				if (showMove)
				{
					if (gui.MoveDownButton())
						moveDown(index);
					if (gui.MoveUpButton())
						moveUp(index);
				}

				if (showRemove && gui.RemoveButton("element", MiniButtonStyle.ModRight))
					remove(index);
			}
			gui._endH();
		}

		private void DrawHeader(bool showAdd, Action add, Action remove, bool showAdvanedToggle)
		{
			gui._beginH();
			{
				foldout = gui.Foldout(sequenceName, foldout, GUILayout.ExpandWidth(true));

				gui.FlexibleSpace();

				if (showAdvanedToggle)
					IsAdvancedChecked = gui.CheckButton(IsAdvancedChecked, "advanced mode");

				gui._beginState(dmValue.Count > 0);
				{
					if (gui.RemoveButton("last element"))
						remove();
				}
				gui._endState();

				if (showAdd && gui.AddButton("element", MiniButtonStyle.ModRight))
					add();
			}
			gui._endH();
		}

		#endregion

		private ElementInfo<TElement> GetElement(int index)
		{
			if (index >= elements.Count)
			{
				var newElement = new ElementInfo<TElement>(
					@id            : id + index,
					@attributes    : attributes.ToArray(),
					@name          : string.Empty,
					@elementType   : elementType,
					@declaringType : targetType,
					@reflectedType : GetType()
				) { Index = index };

				elements.Add(newElement);
			}

			var e   = elements[index];
			e.List  = dmValue;
			e.Index = index;
			return e;
		}

		// List ops
		#region
		protected abstract void Clear();
		protected abstract void RemoveAt(int atIndex);
		protected abstract void Insert(int index, TElement value);

		private void Shuffle()
		{
			dmValue.Shuffle();
		}

		private void MoveElementDown(int i)
		{
			dmValue.MoveElementDown(i);
		}

		private void MoveElementUp(int i)
		{
			dmValue.MoveElementUp(i);
		}

		private void RemoveLast()
		{
			RemoveAt(dmValue.Count - 1);
		}

		private void SetAt(int index, TElement value)
		{
			if (!dmValue[index].GenericEqual(value))
				dmValue[index] = value;
		}

		private void AddValue(TElement value)
		{
			Insert(dmValue.Count, value);
		}

		private void AddValue()
		{
			AddValue((TElement)elementType.GetDefaultValueEmptyIfString());
		}
		#endregion

		private struct DisplayOptions
		{
			public bool Readonly;
			public bool Advanced;
			public bool LineNumbers;
			public bool PerItemRemove;
			public bool GuiBox;

			public DisplayOptions(SeqOpt options)
			{
				Func<SeqOpt, bool> contains = value =>
					(options & value) != 0;

				Readonly      = contains(SeqOpt.Readonly);
				Advanced      = contains(SeqOpt.Advanced);
				LineNumbers   = contains(SeqOpt.LineNumbers);
				PerItemRemove = contains(SeqOpt.PerItemRemove);
				GuiBox        = contains(SeqOpt.GuiBox);
			}
		}
	}

	public class ArrayDrawer<T> : SequenceDrawer<T[], T>
	{
		protected override T[] GetNew()
		{
			return new T[0];
		}

		protected override void RemoveAt(int atIndex)
		{
			dmValue = dmValue.ToList().RemoveAtAndGet(atIndex).ToArray();
		}

		protected override void Clear()
		{
			dmValue = dmValue.ToList().ClearAndGet().ToArray();
		}

		protected override void Insert(int index, T value)
		{
			dmValue = dmValue.ToList().InsertAndGet(index, value).ToArray();
			foldout = true;
		}
	}

	public class ListDrawer<T> : SequenceDrawer<List<T>, T>
	{
		protected override List<T> GetNew()
		{
			return new List<T>();
		}

		protected override void RemoveAt(int index)
		{
			dmValue.RemoveAt(index);
		}

		protected override void Clear()
		{
			dmValue.Clear();
		}

		protected override void Insert(int index, T value)
		{
			dmValue.Insert(index, value);
			foldout = true;
		}
	}
}