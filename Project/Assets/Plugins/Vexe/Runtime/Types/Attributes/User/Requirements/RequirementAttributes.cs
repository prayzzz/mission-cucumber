using UnityEngine;
using System;

namespace Vexe.Runtime.Types
{
	/// <summary>
	/// Tag a field this attribute to denote that it's required (should be assigned)
	/// If the component is not assigned, It will search the scene and try to find a matching component
	/// it will assign the first result it finds
	/// If it couldn't assign it you'll get a warning message saying that you should assign
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class RequiredAttribute : CompositeAttribute
	{
		/// <summary>
		/// Adds the component if it didn't exist
		/// </summary>
		public bool Add { get; set; }

		public RequiredAttribute(int id, bool add) : base(id)
		{
			Add = add;
		}

		public RequiredAttribute(bool add) : this(-1, add)
		{
		}

		public RequiredAttribute()
		{
			id = -1;
		}
	}

	/// <summary>
	/// Tag a field/property with this attribute to denote that it's required (should be assigned) from this gameObject
	/// If it's not assigned you'll get a warning message
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class RequiredFromThisAttribute : RequiredAttribute
	{
		public RequiredFromThisAttribute(int id, bool add) : base(id, add)
		{
		}

		public RequiredFromThisAttribute(bool add) : base(add)
		{
		}

		public RequiredFromThisAttribute()
		{
		}
	}

	public abstract class RequiredFromRelativeAttribute : RequiredFromThisAttribute
	{
		/// <summary>
		/// The relative (child or parent) path to add to (or create and add) if the requirement was not found
		/// If you want to specify a nested relative, you could write its full path, ex:
		/// "Senses/Hearing" - in general "relative/Grandrelative/GGrelative, etc"
		/// </summary>
		public string Path { get; set; }

		/// <summary>
		/// True to create any missing relative within the specified relativePath
		/// </summary>
		public bool Create { get; set; }
	}

	/// <summary>
	/// Tag a field/property with this attribute to denote that it's required (should be assigned) from parent gameObjects
	/// If it's not assigned you'll get a warning message
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class RequiredFromParentsAttribute : RequiredFromRelativeAttribute
	{
		public RequiredFromParentsAttribute() : this(-1)
		{
		}

		public RequiredFromParentsAttribute(int id)
		{
			this.id = id;
		}
	}

	/// <summary>
	/// Tag a field/property with this attribute to denote that it's required (should be assigned) from children gameObjects
	/// If it's not assigned you'll get a warning message
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class RequiredFromChildrenAttribute : RequiredFromRelativeAttribute
	{
		public RequiredFromChildrenAttribute() : this(-1)
		{
		}

		public RequiredFromChildrenAttribute(int id)
		{
			this.id = id;
		}
	}
}