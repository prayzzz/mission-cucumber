using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Vexe.Editor.Framework.Drawers
{
	[CoreDrawer]
	public class NullableDrawer<T> : ObjectDrawer<T?> where T : struct
	{
		private DataMember nullableMember;

		protected override void OnInitialized()
		{
			var info = new DataInfo(
				@id            : id,
				@getter        : dataMember.GetRaw,
				@setter        : dataMember.SetRaw,
				@attributes    : attributes,
				@name          : niceName,
				@elementType   : typeof(T),
				@declaringType : targetType,
				@reflectedType : GetType()
			);

			nullableMember = new DataMember(info, rawTarget);
		}

		public override void OnGUI()
		{
			if (!dmValue.HasValue)
				dmValue = (T)typeof(T).GetDefaultValue();

			MemberField(nullableMember);
		}
	}
}