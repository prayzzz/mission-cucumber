﻿using UnityEngine;

namespace Vexe.Runtime.Types.Examples
{
	public class InlineExample : BetterBehaviour
	{
		[Inline]
		public Transform t;

		// This is a very common combination to use
		// We require the component (in this case from this gameObject),
		// if it doesn't exist it gets added
		// And then we inline it, and hide it from the gameObject
		// NOTE: Both of these are composite attributes,
		// meaning they have an order in which they are drawn
		// RequiredFromThis should be drawn before Inline
		// Otherwise Inline will try to hide a target that's not there yet!
		// So we use an id of 0 to the first attribute, and 1 to the second.
		// 0 comes before 1 :)
		[RequiredFromThis(0, true), Inline(1, HideTarget = true)]
		public BoxCollider col;

		// inlining a game object will draw the editor for all its components
		[Inline]
		public GameObject GO { get; set; }
	}
}