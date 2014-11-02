using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Categories;

using Vexe.Runtime.Extensions;

namespace Assets.Plugins.Editor.Vexe.CustomEditors.Internal
{
    public class CategoryMembersResolver : MembersResolver
    {
        public override MemberGroup Resolve(MemberGroup input, DefineCategoryAttribute definition)
        {
            var output = this.newGroup();
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