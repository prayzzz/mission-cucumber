using System;
using System.Linq;
using UnityEngine;

namespace Vexe.Runtime.Types
{
	/// <summary>
	/// Annotate a string with this attribute to have its value selected from a popup
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class PopupAttribute : DrawnAttribute
	{
		/// <summary>
		/// Use this if you want to dynamically generate the popup values instead of having to hardcode them
		/// The method should have no parameters, and a return type of an array of string, float or int
		/// </summary>
		public string PopulateFrom { get; set; }

		/// <summary>
		/// The popup values
		/// </summary>
		public readonly string[] values;

		public PopupAttribute(string populateFrom)
		{
			PopulateFrom = populateFrom;
		}

		public PopupAttribute(params string[] strings)
		{
			values = strings;
		}

		public PopupAttribute(params int[] ints)
		{
			values = ints.Select(i => i.ToString()).ToArray();
		}

		public PopupAttribute(params float[] floats)
		{
			values = floats.Select(f => f.ToString()).ToArray();
		}
	}
}