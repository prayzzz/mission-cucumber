using System;

using Assets.Plugins.Editor.Vexe.Drawers.API.Base;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Requirements;

using UnityEngine;

using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

using Object = UnityEngine.Object;

namespace Assets.Plugins.Editor.Vexe.Drawers.User.Requirements
{
    public abstract class BaseRequirementAttributeDrawer<T> : CompositeDrawer<Object, T> where T : RequiredAttribute
    {
        private bool isMemberGo;
        private Func<GameObject> mGetSource;

        protected Func<GameObject> getSource
        {
            get
            {
                if (this.mGetSource == null)
                {
                    var goProp = this.targetType.GetProperty("gameObject");
                    this.mGetSource = new Func<GameObject>(() => goProp.GetGetMethod().Invoke(this.rawTarget, null) as GameObject).Memoize();
                }
                return this.mGetSource;
            }
        }

        protected override void OnInitialized()
        {
            this.isMemberGo = this.memberType.IsA<GameObject>();
            if (!this.isMemberGo && this.targetType.GetProperty("gameObject") == null)
                throw new InvalidOperationException("Member `" + this.memberInfo.Name + "` is not a gameObject nor does the target it's in have a gameObject property");
        }

        public override void OnUpperGUI()
        {
            if (this.dataMember.IsNull())
            {
                var from = this.getSource();
                this.dmValue = this.isMemberGo ? (Object)this.GetGO(from) : this.GetComponent(from, this.memberType);
            }
        }

        protected abstract Component GetComponent(GameObject from, Type componentType);
        protected abstract GameObject GetGO(GameObject from);
    }
}