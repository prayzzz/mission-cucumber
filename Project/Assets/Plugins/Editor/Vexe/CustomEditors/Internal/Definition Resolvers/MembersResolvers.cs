using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Vexe.Editor.BetterBehaviourInternal
{
	public class CategoryMembersResolver : MembersResolver
	{
		public override MemberGroup Resolve(MemberGroup input, DefineCategoryAttribute definition)
		{
			var output = newGroup();
			output.AddMembers(input, m =>
			{
				var memberDef = m.Info.GetCustomAttribute<CategoryAttribute>();
				if (memberDef != null)
				{
					m.DisplayOrder = memberDef.displayOrder;
					return memberDef.name == definition.FullPath;
				}
				return false;
			});
			return output;
		}
	}
}