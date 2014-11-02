using Assets.Plugins.Editor.Vexe.Drawers.API.Base;

using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Assets.Plugins.Editor.Vexe.Drawers.API.Core
{
    [CoreDrawer]
    public class NullableDrawer<T> : ObjectDrawer<T?> where T : struct
    {
        private DataMember nullableMember;

        protected override void OnInitialized()
        {
            var info = new DataInfo(
                id            : this.id,
                getter        : this.dataMember.GetRaw,
                setter        : this.dataMember.SetRaw,
                attributes    : this.attributes,
                name          : this.niceName,
                elementType   : typeof(T),
                declaringType : this.targetType,
                reflectedType : this.GetType()
            );

            this.nullableMember = new DataMember(info, this.rawTarget);
        }

        public override void OnGUI()
        {
            if (!this.dmValue.HasValue)
                this.dmValue = (T)typeof(T).GetDefaultValue();

            this.MemberField(this.nullableMember);
        }
    }
}