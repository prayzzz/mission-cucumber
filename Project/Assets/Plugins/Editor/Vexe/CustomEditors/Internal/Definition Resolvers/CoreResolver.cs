using System;
using System.Collections.Generic;
using System.Linq;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Vexe.Editor.BetterBehaviourInternal
{
	public class CoreResolver : GroupResolver
	{
		private readonly GroupResolver[] groupResolvers;
		private readonly List<VisibleMember> excluded;
		private DefineCategoryAttribute definition;

		public CoreResolver(GroupResolver[] groupResolvers, Func<MemberGroup> newGroup)
		{
			this.newGroup = newGroup;
			this.groupResolvers = groupResolvers;
			excluded = new List<VisibleMember>();
			groupResolvers.Foreach(r => r.newGroup = newGroup);
		}

		public override MemberGroup Resolve(MemberGroup input, DefineCategoryAttribute definition)
		{
			this.definition = definition;
			return Resolve(input);
		}

		public MemberGroup Resolve(MemberGroup input)
		{
			var result = newGroup();
			var comparer = new MemberComparer();
			var defResolvers = groupResolvers.Where(r => r is DefintionResolver).ToArray();
			var memResolvers = groupResolvers.Except(defResolvers);

			// Takes a sequence of resolvers - have them each resolve the input
			// and finally return groups of the resolved members
			Func<IEnumerable<GroupResolver>, IEnumerable<List<VisibleMember>>> resolveGroupMembers =
				resolvers => resolvers.Select(r => r.Resolve(input, definition))
									       .Where(g => !g.Members.IsEmpty())
									       .Select(g => g.Members);

			// Solve category definition members
			var defMembers = resolveGroupMembers(defResolvers).Cast<IEnumerable<VisibleMember>>().ToArray();
			if (!defMembers.IsEmpty())
			{
				switch (definition.Grouping)
				{
					case SetOp.Intersection:
						result.AddMembers(defMembers.Aggregate((g1, g2) => g1.Intersect(g2, comparer)));
						break;
					case SetOp.Union:
						result.AddMembers(defMembers.UnionAll(comparer));
						break;
				}
			}

			// Solve members annotated with CategoryMember
			resolveGroupMembers(memResolvers).Foreach(result.UnionMembers);

			// Filter out excluded members
			result.Members.RemoveAll(excluded.Contains);

			// If this definition's members are exclusive (doesn't allow dups)
			// we maintain a ref to its members to exclude them from other defs
			if (definition.Exclusive)
				excluded.AddRange(result.Members);

			return result;
		}
	}
}