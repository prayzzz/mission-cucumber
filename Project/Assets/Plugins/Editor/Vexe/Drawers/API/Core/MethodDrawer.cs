using System;
using System.Reflection;

using Assets.Plugins.Editor.Vexe.Drawers.API.Base;
using Assets.Plugins.Editor.Vexe.GUI;

using Fasterflect;

using UnityEngine;

using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Assets.Plugins.Editor.Vexe.Drawers.API.Core
{
    public class MethodDrawer : BaseDrawer
    {
        private MethodInfo method;
        private DataInfo[] argMembers;
        private object[] argValues;
        private MethodInvoker invoke;

        protected override void OnInitialized()
        {
            this.method       = this.memberInfo as MethodInfo;
            this.invoke        = this.method.DelegateForCallMethod();
            var argInfos = this.method.GetParameters();
            int len      = argInfos.Length;
            this.argValues    = new object[len];
            this.argMembers   = new DataInfo[len];

            for (int i = 0; i < len; i++)
            {
                var arg = argInfos[i];
                var argType = arg.ParameterType;

                this.argValues[i] = argInfos[i].ParameterType.GetDefaultValueEmptyIfString();

                this.argMembers[i] = new DataInfo(
                        id            : this.id + argType.FullName + arg.Name,
                        getter        : () => this.argValues[i],
                        setter        : x => this.argValues[i] = x,
                        attributes    : new Attribute[0],
                        name          : arg.Name,
                        elementType   : argType,
                        declaringType : this.targetType,
                        reflectedType : this.GetType()
                    );
            }

            dbgLog("Method drawer init");
        }

        public override void OnGUI()
        {
            if (!this.Header())
                return;

            this.gui._beginIndent();
            {
                for (int i = 0; i < this.argMembers.Length; i++)
                {
                    this.gui.MemberField(this.argMembers[i], this.rawTarget, this.unityTarget, this.id);
                }
            }
            this.gui._endIndent();
        }

        private bool Header()
        {
            this.gui._beginH();
            {
                if (this.argMembers.Length == 0)
                    this.gui.Label(this.niceName);
                else
                    this.foldout = this.gui.Foldout(this.niceName, this.foldout, GUILayout.ExpandWidth(true));
                this.gui.FlexibleSpace();
                if (this.gui.MiniButton("Invoke", MiniButtonStyle.Right, null))
                    this.invoke(this.rawTarget, this.argValues);
            }
            this.gui._endH();
            return this.argMembers.Length == 0 || this.foldout;
        }
    }
}