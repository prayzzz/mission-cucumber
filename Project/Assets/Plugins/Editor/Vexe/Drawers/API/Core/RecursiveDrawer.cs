using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Assets.Plugins.Editor.Vexe.Drawers.API.Base;
using Assets.Plugins.Editor.Vexe.Other;
using Assets.Plugins.Vexe.Runtime.Serialization;

using Fasterflect;

using Smooth.Slinq;

using UnityEditor;

using UnityEngine;

using Vexe.Editor.Helpers;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Helpers;
using Vexe.Runtime.Types;

using Object = UnityEngine.Object;

namespace Assets.Plugins.Editor.Vexe.Drawers.API.Core
{
    [CoreDrawer]
    public class RecursiveDrawer<T> : ObjectDrawer<T>
    {
        private int childCount;
        private string nullString;

        protected override void OnInitialized()
        {
            this.childCount = this.memberType.GetChildren().Count();
            this.nullString = string.Format("null ({0})", this.memberTypeName);
        }

        public override void OnGUI()
        {
            if (!this.DrawField()) return;

            if (this.dataMember.IsNull())
            {
                this.gui.HelpBox("Member value is null");
                return;
            }

            var members = this.GetMembers().Invoke(this.dmValue.GetType());
            if (members.IsEmpty())
            {
                this.gui.HelpBox("Object doesn't have any visible members");
                return;
            }

            this.gui._beginIndent();
            {
                for (int i = 0; i < members.Count; i++)
                    this.gui.MemberField(members[i], this.dmValue, this.unityTarget, this.id, false);
            }
            this.gui._endIndent();
        }

        Func<T, string> _getFieldString;
        Func<T, string> GetFieldString()
        {
            return this._getFieldString ?? (this._getFieldString = new Func<T, string>(value =>
            {
                var field = string.Format("{0} ({1})", value.GetType().GetFriendlyName(), this.memberTypeName);
                var obj = value as Object;
                if (obj != null)
                    field = field.Insert(0, obj.name + " - ");
                return field;
            }).Memoize());
        }

        Func<Type, List<MemberInfo>> _getMembers;
        Func<Type, List<MemberInfo>> GetMembers()
        {
            return this._getMembers ?? (this._getMembers = new Func<Type, List<MemberInfo>>(type =>
                        type.GetMembers(Flags.InstanceAnyVisibility).Slinq()
                             .Where(SerializationLogic.Default.IsVisibleMember)
                             .OrderBy(m => m.GetDataType().Name)
                             .OrderBy(m => m.Name)
                             .ToList()
                        ).Memoize());
        }

        private bool DrawField()
        {
            this.gui._beginH();
            {
                this.Foldout();
                this.gui.Space(-10.5f);
                this.Field();
                this.Buttons();
            }
            this.gui._endH();

            return this.foldout;
        }

        private void Field()
        {
            var value = this.dmValue;
            if (this.memberType.IsA<Object>())
            {
                var obj  = value as Object;
                this.dmValue  = (T)(object)this.gui.ObjectField(this.niceName, obj);
                var rect = this.gui.GetLastRect();
                {
                    GUIHelper.PingField(rect, obj, MouseCursor.Link);
                    GUIHelper.SelectField(rect, obj, EventsHelper.MouseEvents.IsRMB_MouseDown(), MouseCursor.Link);
                }
            }
            else
            {
                string field = value == null ? this.nullString : this.GetFieldString().Invoke(value);
                this.gui.TextField(this.niceName, field);
                if (value == null && !this.memberType.IsAbstract)
                    this.TryCreateInstance(this.memberType);
            }
        }

        protected virtual void Buttons()
        {
            if (this.childCount <= 1)
            {
                this.gui._beginState(false);
                {
                    this.gui.SelectionButton("Object doesn't have any children/implementers");
                }
                this.gui._endState();
                return;
            }

            var tabs = new List<Tab>();

            Action<Func<Type[]>, Action<Type>, string> newTypeTab = (getValues, create, title) =>
                tabs.Add(new Tab<Type>(
                    getValues    : getValues,
                    getCurrent   : () => { var x = this.dmValue; return x == null ? null : x.GetType(); },
                    setTarget    : newType => { if (newType == null) this.dmValue = default(T); else create(newType); },
                    getValueName : type => type.Name,
                    title        : title
                ));

            if (this.memberType.IsInterface)
            {
                Action<Func<Object[]>, string> newUnityTab = (getValues, title) =>
                    tabs.Add(new Tab<Object>(
                        getValues    : getValues,
                        @getCurrent   : this.dataMember.As<Object>,
                        setTarget    : this.dataMember.SetRaw,
                        getValueName : obj => obj.name + " (" + obj.GetType().Name + ")",
                        title        : title
                    ));

                newUnityTab(() => Object.FindObjectsOfType<Object>()
                                                      .OfType(this.memberType)
                                                      .ToArray(), "Scene");

                newUnityTab(() => PrefabHelper.GetComponentPrefabs(this.memberType)
                                                        .ToArray(), "Prefabs");

                newTypeTab(() => ReflectionHelper.GetAllUserTypesOf(this.memberType)
                                                           .Where(t => t.IsA<Object>())
                                                           .Where(t => !t.IsAbstract)
                                                           .ToArray(), this.TryCreateInstanceInGO, "Unity types");
            }

            newTypeTab(() => ReflectionHelper.GetAllUserTypesOf(this.memberType)
                                                        .Disinclude(this.memberType.IsAbstract ? this.memberType : null)
                                                        .Where(t => !t.IsA<Object>())
                                                        .ToArray(), this.TryCreateInstance, "System types");

            if (this.gui.SelectionButton())
                SelectionWindow.Show("Select a `" + this.memberTypeName + "` object", tabs.ToArray());
        }

        private void TryCreateInstanceInGO(Type newType)
        {
            this.TryCreateInstance(() => new GameObject("(new) " + newType.Name).AddComponent(newType));
        }

        private void TryCreateInstance(Type newType)
        {
            this.TryCreateInstance(() => newType.Instance<T>(this.rawTarget));
        }

        private void TryCreateInstance(Func<object> create)
        {
            try
            {
                this.dataMember.SetRaw(create());
                EditorHelper.RepaintAllInspectors();
                this.foldout = true;
            }
            catch (TargetInvocationException e)
            {
                Debug.LogError(string.Format("Couldn't create instance of type {0}. Make sure the type has an empty constructor. Message: {1}, Stacktrace: {2}", this.memberTypeName, e.Message, e.StackTrace));
            }
        }
    }
}
