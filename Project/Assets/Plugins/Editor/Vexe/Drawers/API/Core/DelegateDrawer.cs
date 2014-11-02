using System;
using System.Linq;
using System.Reflection;

using Assets.Plugins.Editor.Vexe.Drawers.API.Base;
using Assets.Plugins.Editor.Vexe.GUI;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;
using Assets.Plugins.Vexe.Runtime.Types.Delegates;

using Fasterflect;

using UnityEditor;

using UnityEngine;

using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Assets.Plugins.Editor.Vexe.Drawers.API.Core
{
    [CoreDrawer]
    public class DelegateDrawer : ObjectDrawer<BaseDelegate>
    {
        private AddingData adding;
        private DataMember[] argMembers;
        private object[] argValues;
        private string[] ignored;
        private string headerString;

        protected override void OnInitialized()
        {
            this.ignored = DelegateSettings.IgnoredMethods;

            // initialize adding area and foldout keys
            this.adding       = new AddingData();
            this.kAdvancedKey = this.id + "advanced";
            this.kAddKey      = this.id + "add";
            this.kInvokeKey   = this.id + "invoke";
            this.headerString = string.Format("{0} ({1})", this.niceName, this.memberTypeName);

            // make sure delegate is not null
            if (this.dmValue == null)
                 this.dmValue = this.memberType.Instance<BaseDelegate>();

            // initialize arguments data (used when invoking the delegate from the editor)
            int len      = this.dmValue.ParamTypes.Length;
            this.argValues    = new object[len];
            this.argMembers   = new DataMember[len];

            for (int iLoop = 0; iLoop < len; iLoop++)
            {
                int i = iLoop;
                var paramType = this.dmValue.ParamTypes[i];

                this.argValues[i] = paramType.GetDefaultValueEmptyIfString();

                var elementInfo = new DataInfo(
                        id            : this.id + paramType.FullName + i,
                        getter        : () => this.argValues[i],
                        setter        : x => this.argValues[i] = x,
                        attributes    : new Attribute[0],
                        name          : string.Format("({0})", paramType.GetFriendlyName()),
                        elementType   : paramType,
                        declaringType : this.targetType,
                        reflectedType : this.GetType()
                    );

                this.argMembers[i] = new DataMember(elementInfo, this.rawTarget);
            }

            dbgLog("Delegate drawer init");
        }

        public override void OnGUI()
        {
            this.foldout = this.gui.Foldout(this.headerString, this.foldout, GUILayout.ExpandWidth(true));
            if (!this.foldout) return;

            // read
            var handlers = this.dmValue.handlers;

            this.gui._beginV(UnityEngine.GUI.skin.box);
            {

                // header
                {
                    this.gui._beginH();
                    {
                        this.gui.BoldLabel("Target :: Handler");
                        this.gui._beginState(this.foldout && handlers.Count > 0);
                        {
                            this.chkAdvanced = this.gui.CheckButton(this.chkAdvanced, "advanced mode", MiniButtonStyle.Right);
                        }
                        this.gui._endState();
                    }
                    this.gui._endH();
                }

                this.gui.Splitter();

                // body
                {
                    // target : handler
                    if (handlers.Count == 0)
                    {
                        this.gui.HelpBox("There are no handlers for this delegate");
                    }
                    else
                    {
                        for (int iLoop = 0; iLoop < handlers.Count; )
                        {
                            var i = iLoop;
                            var handler = handlers[i];
                            var removed = false;
                            this.gui._beginH();
                            {
                                this.gui.ObjectField(handler.target);
                                this.gui.TextField(handler.method);
                                if (this.chkAdvanced)
                                {
                                    if (this.gui.MoveDownButton())
                                        handlers.MoveElementDown(i);
                                    if (this.gui.MoveUpButton())
                                        handlers.MoveElementUp(i);
                                    if (this.gui.RemoveButton("handler", MiniButtonStyle.ModRight))
                                    {
                                        handlers.RemoveAt(i);
                                        removed = true;
                                    }
                                }
                            }
                            this.gui._endH();
                            if (!removed) iLoop++;
                        }
                    }
                }

                this.gui.Splitter();

                // footer
                {
                    // add> gameObject :: invoke>
                    this.gui._beginH();
                    {
                        this.gui._beginState(this.foldoutAdd && this.adding.gameObject != null && this.adding.component != null && this.adding.method != null);
                        {
                            if (this.gui.Button("Add", "Add new handler", EditorStyles.miniButton, GUILayout.Width(40f)))
                            {
                                handlers.Add(new BaseDelegate.Handler
                                {
                                    target = this.adding.component,
                                    method = this.adding.method.Name
                                });
                            }
                        }
                        this.gui._endState();

                        this.gui.Space(10f);
                        this.foldoutAdd = this.gui.Foldout(this.foldoutAdd);
                        // gameObject
                        this.gui.FlexibleSpace();
                        var newGo = this.gui.ObjectField(this.adding.gameObject);
                        {
                            if (this.adding.gameObject != newGo)
                            {
                                this.adding.gameObject = newGo;
                                this.foldoutAdd = true;
                            }
                        }
                        this.gui.FlexibleSpace();
                        bool paramless = this.argMembers.IsEmpty();
                        this.gui._beginState(this.foldoutInvoke || paramless);
                        {
                            if (this.gui.Button("Invoke", EditorStyles.miniButtonRight, GUILayout.Width(50f)))
                            {
                                for (int i = 0; i < handlers.Count; i++)
                                {
                                    var handler = handlers[i];
                                    var method = handler.target.GetType().DelegateForCallMethod(handler.method, this.dmValue.ParamTypes);
                                    method.Invoke(handler.target, this.argValues);
                                }
                            }
                        }
                        this.gui._endState();
                        this.gui.Space(8f);
                        if (!paramless && handlers.Count > 0)
                            this.foldoutInvoke = this.gui.Foldout(this.foldoutInvoke);
                    }
                    this.gui._endH();

                    // adding area: component -- method
                    if (this.foldoutAdd && this.adding.gameObject != null)
                    {
                        this.gui.Space(5f);
                        this.gui.Label("Add handler:");
                        this.gui._beginIndent(UnityEngine.GUI.skin.textArea);
                        {
                            // component
                            if (this.adding.gameObject != null)
                            {
                                var components   = this.adding.gameObject.GetAllComponents();
                                int cIndex       = components.IndexOfZeroIfNotFound(this.adding.component);
                                var uniqueNames  = components.Select(c => c.GetType().Name).ToList().Uniqify();
                                var targetSelection = this.gui.Popup("Target", cIndex, uniqueNames.ToArray());
                                {
                                    if (cIndex == targetSelection && this.adding.component == components[targetSelection]) return;
                                    this.adding.component = components[targetSelection];
                                }

                                // method
                                if (this.adding.component != null)
                                {
                                    var methods = this.adding.component.GetType()
                                                                            .GetMethods(this.dmValue.ReturnType, this.dmValue.ParamTypes, Flags.InstancePublic, false)
                                                                            .Where(m => !m.IsDefined<HideAttribute>())
                                                                            .Where(m => !this.ignored.Contains(m.Name))
                                                                            .ToList();
                                    int mIndex  = methods.IndexOfZeroIfNotFound(this.adding.method);
                                    int methodSelection = this.gui.Popup("Handler", mIndex, methods.Select(m => m.GetFullName()).ToArray());
                                    {
                                        if (methods.IsEmpty() || (mIndex == methodSelection && this.adding.method == methods[methodSelection])) return;
                                        this.adding.method = methods[methodSelection];
                                    }
                                }
                            }
                        }
                        this.gui._endIndent();
                    }

                    // invocation args
                    if (this.foldoutInvoke)
                    {
                        this.gui.Label("Invoke with arguments:");
                        this.gui._beginIndent();
                        {
                            for (int i = 0; i < this.argMembers.Length; i++)
                                this.gui.MemberField(this.argMembers[i], this.unityTarget, this.id, false);
                        }
                        this.gui._endIndent();
                    }
                }
            }
            this.gui._endV();

            // write
            this.dmValue.handlers = handlers;
        }

        private struct AddingData
        {
            public GameObject gameObject;
            public Component component;
            public MethodInfo method;
        }

        // Keys & foldouts
        #region
        private string kAdvancedKey;
        private string kAddKey;
        private string kInvokeKey;

        private bool foldoutAdd
        {
            get { return this.prefs.GetSafeBool(this.kAddKey);}
            set { this.prefs.SetBool(this.kAddKey, value); }
        }
        private bool chkAdvanced
        {
            get { return this.prefs.GetSafeBool(this.kAdvancedKey); }
            set { this.prefs.SetBool(this.kAdvancedKey, value); }
        }
        private bool foldoutInvoke
        {
            get { return this.prefs.GetSafeBool(this.kInvokeKey); }
            set { this.prefs.SetBool(this.kInvokeKey, value); }
        }
        #endregion
    }
}