#define PROFILE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Plugins.Editor.Vexe.Drawers.API.Base;
using Assets.Plugins.Editor.Vexe.GUI;
using Assets.Plugins.Editor.Vexe.Other;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Others;

using UnityEditor;

using UnityEngine;

using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

using Object = UnityEngine.Object;

namespace Assets.Plugins.Editor.Vexe.Drawers.API.Core
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
            get { return this.prefs.GetSafeBool(this.advancedKey); }
            set { this.prefs.SetBool(this.advancedKey, value); }
        }

        protected abstract TSequence GetNew();

        public SequenceDrawer()
        {
            this.elementType = typeof(TElement);
            this.elements    = new List<ElementInfo<TElement>>();
        }

        protected override void OnInitialized()
        {
            var seqOptions       = this.attributes.GetAttribute<SeqAttribute>();
            this.options              = new DisplayOptions(seqOptions != null ? seqOptions.options : SeqOpt.None);
            this.options.Advanced     = this.options.Advanced || this.attributes.AnyIs<AdvancedAttribute>();
            this.options.Readonly     = this.options.Readonly || this.attributes.AnyIs<ReadonlyAttribute>();

            // Sequence name
            { 
                var builder = new StringBuilder();
                builder.Append(this.niceName);
                builder.Append(" (");
                builder.Append(this.memberType.GetFriendlyName());
                builder.Append(")");
                if (this.options.Readonly) 
                    builder.Append(" (Readonly)");
                this.sequenceName = builder.ToString();
            }

            this.advancedKey          = this.id + this.sequenceName + "advanced";
            this.perItemDrawing       = this.attributes.AnyIs<PerItemAttribute>();
            this.shouldDrawAddingArea = !this.options.Readonly && this.elementType.IsA<Object>();

            if (this.dmValue == null)
                this.dmValue = this.GetNew();
        }

        public override void OnGUI()
        {
            bool showAdvanced = this.options.Advanced && !this.options.Readonly;

            this.DrawHeader(
                showAdd           : !this.options.Readonly,
                add               : this.AddValue,
                remove            : this.RemoveLast,
                showAdvanedToggle : showAdvanced
            );

            if (!this.foldout) return;

            if (this.dmValue.IsEmpty())
            {
                this.gui.HelpBox("Sequence is empty");
                return;
            }

            this.gui._beginV(this.options.GuiBox ? UnityEngine.GUI.skin.box : GUIStyle.none);
            {
                if (this.IsAdvancedChecked)
                    this.DrawAdvancedArea();

                this.gui._beginIndent(this.options.GuiBox ? UnityEngine.GUI.skin.box : GUIStyle.none);
                {
#if PROFILE
                    Profiler.BeginSample("Sequence Elements");
#endif
                    for (int i= 0; i < this.dmValue.Count; i++)
                    {
                        var current = this.dmValue[i];
                        this.DrawField(
                            index        : i,
                            value        : current,
                            showLineNums : this.options.LineNumbers,
                            showInspect  : this.IsAdvancedChecked,
                            showMove     : this.IsAdvancedChecked && showAdvanced,
                            moveDown     : this.MoveElementDown,
                            moveUp       : this.MoveElementUp,
                            showRemove   : !this.options.Readonly && this.options.PerItemRemove,
                            remove       : this.RemoveAt
                        );
                    }
#if PROFILE
                    Profiler.EndSample();
#endif
                }
                this.gui._endIndent();
            }
            this.gui._endV();

            if (this.shouldDrawAddingArea)
                this.DrawAddingArea();
        }

        // Drawings
        #region

        private void DrawAdvancedArea()
        {
            this.gui._beginIndent(UnityEngine.GUI.skin.box);
            {
                this.gui._beginH();
                {
                    this.newSize = this.gui.IntField("New size", this.newSize);
                    if (this.gui.MiniButton("c", "Commit", MiniButtonStyle.ModRight))
                    {
                        if (this.newSize != this.dmValue.Count)
                            this.dmValue.AdjustSize(this.newSize, this.RemoveAt, this.AddValue);
                    }
                }
                this.gui._endH();

                this.gui._beginH();
                {
                    this.gui.Label("Commands");
                    if (this.gui.MiniButton("Shuffle", "Shuffle list (randomize the order of the list's elements", (GUILayoutOption[])null))
                        this.Shuffle();
                    if (this.gui.MoveDownButton())
                        this.dmValue.Shift(true);
                    if (this.gui.MoveUpButton())
                        this.dmValue.Shift(false);
                    Action<Predicate<TElement>> clearSection = predicate =>
                    {
                        for (int i = this.dmValue.Count - 1; i > -1; i--)
                            if (predicate(this.dmValue[i]))
                                this.RemoveAt(i);
                    };

                    if (!this.elementType.IsValueType && this.gui.MiniButton("N", "Filter nulls"))
                         clearSection(e => e == null);
                    if (this.gui.ClearButton("elements", MiniButtonStyle.ModRight))
                        this.Clear();
                }
                this.gui._endH();
            }
            this.gui._endIndent();
        }

        private void DrawAddingArea()
        {
            Action<Object> addOnDrop = obj =>
            {
                var go = obj as GameObject;
                object value = go != null ? go.GetComponent(this.elementType) : obj;
                this.AddValue((TElement)value);
            };

            this.gui._beginIndent();
            {
                this.gui.DragDropArea<Object>(
                    label: "+",
                    labelSize: 14,
                    style: EditorStyles.toolbarButton,
                    canSetVisualModeToCopy: dragObjects => dragObjects.All(o =>
                    {
                        var go = o as GameObject;
                        return go == null ? o.GetType().IsA(this.elementType) : go.GetComponent(this.elementType) != null;
                    }),
                    cursor: MouseCursor.Link,
                    onDrop: addOnDrop,
                    @onMouseUp: () => SelectionWindow.Show(new Tab<Object>(
                        getValues: () => Object.FindObjectsOfType(this.elementType),
                        getCurrent: () => null,
                        setTarget: item =>
                        {
                            this.AddValue((TElement)(object)item);
                        },
                        getValueName: value => value.name,
                        title: this.elementType.Name + "s")),
                    preSpace: 2f,
                    postSpace: 35f,
                    height: .5f
                );
            }
            this.gui._endIndent();
        }

        private Func<int, string> _keyForIndex;
        private Func<int, string> GetKeyForIndex()
        {
            return this._keyForIndex ?? (this._keyForIndex = new Func<int, string>(index => this.id + index).Memoize());
        }

        private void DrawField(int index, TElement value,
            bool showLineNums, bool showInspect,
            bool showMove, Action<int> moveDown, Action<int> moveUp,
            bool showRemove, Action<int> remove)
        {
            this.gui._beginH();
            {
                if (showLineNums)
                    this.gui.NumericLabel(index);

                var previous = value;
                this.gui._beginChange();
                {
                    this.gui._beginV();
                    {
                        this.gui.MemberField(this.GetElement(index), this.rawTarget, this.unityTarget, this.GetKeyForIndex().Invoke(index), !this.perItemDrawing);
                    }
                    this.gui._endV();
                }
                if (this.gui._endChange())
                {
                    if (this.options.Readonly)
                        this.dmValue[index] = previous;
                }

                if (showInspect)
                {
                    var c = value as Component;
                    var go = c == null ? value as GameObject : c.gameObject;
                    if (go != null)
                        this.gui.InspectButton(go);
                }

                if (showMove)
                {
                    if (this.gui.MoveDownButton())
                        moveDown(index);
                    if (this.gui.MoveUpButton())
                        moveUp(index);
                }

                if (showRemove && this.gui.RemoveButton("element", MiniButtonStyle.ModRight))
                    remove(index);
            }
            this.gui._endH();
        }

        private void DrawHeader(bool showAdd, Action add, Action remove, bool showAdvanedToggle)
        {
            this.gui._beginH();
            {
                this.foldout = this.gui.Foldout(this.sequenceName, this.foldout, GUILayout.ExpandWidth(true));

                this.gui.FlexibleSpace();

                if (showAdvanedToggle)
                    this.IsAdvancedChecked = this.gui.CheckButton(this.IsAdvancedChecked, "advanced mode");

                this.gui._beginState(this.dmValue.Count > 0);
                {
                    if (this.gui.RemoveButton("last element"))
                        remove();
                }
                this.gui._endState();

                if (showAdd && this.gui.AddButton("element", MiniButtonStyle.ModRight))
                    add();
            }
            this.gui._endH();
        }

        #endregion

        private ElementInfo<TElement> GetElement(int index)
        {
            if (index >= this.elements.Count)
            {
                var newElement = new ElementInfo<TElement>(
                    id            : this.id + index,
                    attributes    : this.attributes.ToArray(),
                    name          : string.Empty,
                    elementType   : this.elementType,
                    declaringType : this.targetType,
                    reflectedType : this.GetType()
                ) { Index = index };

                this.elements.Add(newElement);
            }

            var e   = this.elements[index];
            e.List  = this.dmValue;
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
            this.dmValue.Shuffle();
        }

        private void MoveElementDown(int i)
        {
            this.dmValue.MoveElementDown(i);
        }

        private void MoveElementUp(int i)
        {
            this.dmValue.MoveElementUp(i);
        }

        private void RemoveLast()
        {
            this.RemoveAt(this.dmValue.Count - 1);
        }

        private void SetAt(int index, TElement value)
        {
            if (!this.dmValue[index].GenericEqual(value))
                this.dmValue[index] = value;
        }

        private void AddValue(TElement value)
        {
            this.Insert(this.dmValue.Count, value);
        }

        private void AddValue()
        {
            this.AddValue((TElement)this.elementType.GetDefaultValueEmptyIfString());
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

                this.Readonly      = contains(SeqOpt.Readonly);
                this.Advanced      = contains(SeqOpt.Advanced);
                this.LineNumbers   = contains(SeqOpt.LineNumbers);
                this.PerItemRemove = contains(SeqOpt.PerItemRemove);
                this.GuiBox        = contains(SeqOpt.GuiBox);
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
            this.dmValue = this.dmValue.ToList().RemoveAtAndGet(atIndex).ToArray();
        }

        protected override void Clear()
        {
            this.dmValue = this.dmValue.ToList().ClearAndGet().ToArray();
        }

        protected override void Insert(int index, T value)
        {
            this.dmValue = this.dmValue.ToList().InsertAndGet(index, value).ToArray();
            this.foldout = true;
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
            this.dmValue.RemoveAt(index);
        }

        protected override void Clear()
        {
            this.dmValue.Clear();
        }

        protected override void Insert(int index, T value)
        {
            this.dmValue.Insert(index, value);
            this.foldout = true;
        }
    }
}