using System;
using System.Linq;
using System.Text.RegularExpressions;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Vexe.Editor.BetterBehaviourInternal
{
	public class PatternResolver : DefintionResolver
	{
		public override MemberGroup Resolve(MemberGroup input, DefineCategoryAttribute definition)
		{
			var output = newGroup();
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
			var output = newGroup();
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
			var output = newGroup();
			Predicate<MemberType> isMemberTypeDefined = mType => (definition.MemberType & mType) > 0;
			output.AddMembers(input, mType => isMemberTypeDefined((MemberType)mType.Info.MemberType));
			return output;
		}
	}

	public class ExplicitMemberAddResolver : DefintionResolver
	{
		public override MemberGroup Resolve(MemberGroup input, DefineCategoryAttribute definition)
		{
			var output = newGroup();
			var explicitMembers = definition.ExplicitMembers;
			output.AddMembers(input, m => explicitMembers.Contains(m.Name));
			return output;
		}
	}
}
