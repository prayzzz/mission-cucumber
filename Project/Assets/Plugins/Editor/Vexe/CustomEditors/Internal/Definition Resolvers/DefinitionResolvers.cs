using System;
using System.Linq;
using System.Text.RegularExpressions;

using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Categories;

using Vexe.Runtime.Extensions;

namespace Assets.Plugins.Editor.Vexe.CustomEditors.Internal
{
    public class PatternResolver : DefintionResolver
    {
        public override MemberGroup Resolve(MemberGroup input, DefineCategoryAttribute definition)
        {
            var output = this.newGroup();
            var pattern = definition.Pattern;
            if (!pattern.IsNullOrEmpty())
            {
                output.AddMembers(input, member => Regex.IsMatch(member.Name, pattern));
            }
            return output;
        }
    }

    public class ReturnTypeResolver : DefintionResolver
    {
        public override MemberGroup Resolve(MemberGroup input, DefineCategoryAttribute definition)
        {
            var output = this.newGroup();
            var returnType = definition.DataType;
            if (returnType != null)
            {
                output.AddMembers(input, m => m.DataType.IsA(returnType));
            }
            return output;
        }
    }

    public class MemberTypesResolver : DefintionResolver
    {
        public override MemberGroup Resolve(MemberGroup input, DefineCategoryAttribute definition)
        {
            var output = this.newGroup();
            Predicate<MemberType> isMemberTypeDefined = mType => (definition.MemberType & mType) > 0;
            output.AddMembers(input, mType => isMemberTypeDefined((MemberType)mType.Info.MemberType));
            return output;
        }
    }

    public class ExplicitMemberAddResolver : DefintionResolver
    {
        public override MemberGroup Resolve(MemberGroup input, DefineCategoryAttribute definition)
        {
            var output = this.newGroup();
            var explicitMembers = definition.ExplicitMembers;
            output.AddMembers(input, m => explicitMembers.Contains(m.Name));
            return output;
        }
    }
}
