using UnityEngine;

using Vexe.Runtime.Types;

namespace Assets.Plugins.Editor.Vexe.Drawers.API.Base
{
    public abstract class ObjectDrawer<T> : BaseDrawer
    {
        protected DataMember<T> dataMember { private set; get; }

        protected T dmValue
        {
            get { return this.dataMember.Value; }
            set { this.dataMember.Value = value; }
        }

        protected override void InternalInitialize()
        {
            dbgLog("Undo target: " + this.unityTarget);
            this.dataMember = DataMember.Create<T>(this.memberInfo, this.rawTarget, this.RegisterDirty, this.RegisterUndo);
        }

        public void MemberField()
        {
            this.MemberField(this.dataMember);
        }

        public void MemberField<TMemberType>(DataMember<TMemberType> dm)
        {
            this.gui.MemberField(dm, this.unityTarget, this.id);
        }

        public sealed override void OnLeftGUI()
        {
        }
        public sealed override void OnRightGUI()
        {
        }
        public sealed override void OnUpperGUI()
        {
        }
        public sealed override void OnLowerGUI()
        {
        }
        public sealed override void OnMemberDrawn(Rect area)
        {
        }
    }
}