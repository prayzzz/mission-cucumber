using System;
using System.Reflection;

using Assets.Plugins.Editor.Vexe.GUI;

using Vexe.Runtime.Extensions;

using Object = UnityEngine.Object;

namespace Assets.Plugins.Editor.Vexe.CustomEditors.Internal
{
    public class VisibleMember : ICanBeDrawn, IEquatable<VisibleMember>
    {
        private readonly MemberInfo info;
        private readonly GLWrapper gui;
        private readonly object rawTarget;
        private readonly Object unityTarget;
        private readonly Type dataType;
        private readonly string key;

        public MemberInfo Info    { get { return this.info; } }
        public Type DataType      { get { return this.dataType; } }
        public string Name        { get { return this.info.Name; } }
        public float DisplayOrder { get; set; }

        public VisibleMember(MemberInfo info, GLWrapper gui, object rawTarget, Object unityTarget, string key)
        {
            this.info        = info;
            this.gui         = gui;
            this.rawTarget   = rawTarget;
            this.unityTarget = unityTarget;
            this.key         = key;
            this.dataType         = info.GetDataType();
        }

        public void Draw()
        {
            this.gui.MemberField(this.info, this.rawTarget, this.unityTarget, this.key, false);
        }

        public void HeaderSpace()
        {
            this.gui.Space(10f);
        }

        public bool Equals(VisibleMember other)
        {
            return this.info.Equals(other.info);
        }
    }
}