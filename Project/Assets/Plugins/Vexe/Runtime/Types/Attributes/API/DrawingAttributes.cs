﻿using System;

namespace Vexe.Runtime.Types
{
	/// <summary>
	/// Inherit this attribute if you want to have attributes that you could composite/decorate
	/// Composite attributes are used by CompositeDrawers
	/// Note that in your composite drawers you should only add decorations and modify the member value
	/// you shouldn't manipulate the way the member is drawn, this is what the DrawnAttribute is for
	/// Example of composite attributes: Comment, Whitespace, Min, Max, Regex, etc
	/// </summary>
	public abstract class CompositeAttribute : Attribute
	{
		public int id { get; set; }

		public CompositeAttribute()
		{
		}

		public CompositeAttribute(int id)
		{
			this.id = id;
		}
	}

	/// <summary>
	/// Inherit this attribute if you want to have attributes that lets you draw your members in a custom way
	/// These attributes are not composed - they are picked by AttributeDrawers to draw your members
	/// Example of those: BetterVectorAttribute, PopupAttribute, TagsAttribute, etc
	/// </summary>
	public abstract class DrawnAttribute : Attribute
	{
	}

	/// <summary>
	/// Inherit from this attribute to define per-element drawing for your collections
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class DefinesElementDrawingAttribute : Attribute { }

	/// <summary>
	/// Annotate sequences (array/list) with this to signify that you want 
	/// your attributes to be applied on each element instead of the sequence itself
	/// </summary>
	[DefinesElementDrawing] public class PerItemAttribute   : Attribute { }

	/// <summary>
	/// Annotate dictionaries with this to signify that you want 
	/// your attributes to be applied on each key instead of the dictionary itself
	/// </summary>
	[DefinesElementDrawing] public class PerKeyAttribute    : Attribute { }

	/// <summary>
	/// Annotate dictionaries with this to signify that you want 
	/// your attributes to be applied on each value instead of the dictionary itself
	/// </summary>
	[DefinesElementDrawing] public class PerValueAttribute  : Attribute { }

	/// <summary>
	/// Annotate dictionaries with this to tell that you want your perkey/pervalue
	/// attributes to be ignored on the adding segment
	/// (the two values (key/value) that you use to add new pairs to the dictionary)
	/// </summary>
	public class IgnoreAddArea : Attribute { }
}