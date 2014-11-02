using System;
using System.Collections.Generic;
using System.Linq;

using Assets.Plugins.Editor.Vexe.Drawers.API.Base;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Others;

using UnityEditor;

using UnityEditorInternal;

using UnityEngine;

using Vexe.Runtime.Extensions;

using Object = UnityEngine.Object;

#pragma warning disable 0168

namespace Assets.Plugins.Editor.Vexe.Drawers.User.Others
{
    public class InlineDrawer : CompositeDrawer<Object, InlineAttribute>
    {
        private UnityEditor.Editor editor;
        private bool hideTarget;
        private Func<GameObject, IEnumerable<UnityEditor.Editor>> mGetEditors;
        private List<bool> expandValues;
        private Object current;

        public bool GuiBox { get; set; }

        public bool HideTarget
        {
            get { return this.hideTarget; }
            set
            {
                if (this.hideTarget != value)
                {
                    this.dmValue.SetVisibility(!value);
                    this.hideTarget = value;
                }
            }
        }

        private Func<GameObject, IEnumerable<UnityEditor.Editor>> getEditors
        {
            get
            {
                if (this.mGetEditors == null)
                {
                    dbgLog("assigning get editors");
                    this.mGetEditors = new Func<GameObject, IEnumerable<UnityEditor.Editor>>(go =>
                    {
                        dbgLog("created new editors");
                        return go.GetAllComponents().Select<Component, UnityEditor.Editor>(UnityEditor.Editor.CreateEditor);
                    }).Memoize();
                }
                return this.mGetEditors;
            }
        }

        protected override void OnInitialized()
        {
            this.expandValues = new List<bool>();
            this.GuiBox       = this.attribute.GuiBox;
            this.HideTarget   = this.attribute.HideTarget;
            dbgLog("Initialized InlineDrawer");
        }

        public override void OnLeftGUI()
        {
            this.Foldout();
            this.gui.Space(-10f);
        }

        public override void OnLowerGUI()
        {
            var objRef = this.dmValue;
            if (this.foldout)
            {
                if (objRef == null)
                {
                    this.gui.HelpBox("Please assign a target to inline");
                    return;
                }

                Action<Object> update = obj =>
                {
                    this.current = obj;
                    this.editor = UnityEditor.Editor.CreateEditor(this.current);
                };

                if (this.editor == null || objRef != this.current)
                    update(objRef);

                this.gui._beginV(this.GuiBox ? UnityEngine.GUI.skin.box : GUIStyle.none);
                {
                    var go = objRef as GameObject;
                    if (go != null)
                    {
                        this.getEditors(go).Foreach(this.DrawEditor);
                    }
                    else
                    {
                        if (this.editor.target == null)
                            update(objRef);

                        this.editor.OnInspectorGUI();
                    }
                }
                this.gui._endV();
            }
        }

        private void DrawEditor(UnityEditor.Editor e, int index)
        {
            this.DrawExpandableHeader(e.target, index, e.OnInspectorGUI);
        }

        private void DrawExpandableHeader(Object target, int index, Action body)
        {
            bool expanded = EditorGUILayout.InspectorTitlebar(InternalEditorUtility.GetIsInspectorExpanded(target), target);
            bool previous;
            if (index >= this.expandValues.Count)
            {
                this.expandValues.Add(expanded);
                previous = !expanded;
            }
            else
            {
                previous = this.expandValues[index];
            }
            if (expanded != previous)
            {
                this.expandValues[index] = expanded;
                InternalEditorUtility.SetIsInspectorExpanded(target, expanded);
            }
            if (expanded)
            {
                this.gui.Space(5f);
                this.gui._beginIndent();
                {
                    body();
                }
                this.gui._endIndent();

            }
        }
    }
}