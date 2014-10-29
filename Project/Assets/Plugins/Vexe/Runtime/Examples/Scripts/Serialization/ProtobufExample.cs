using System;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;

namespace Vexe.Runtime.Types.Examples
{
	[BasicView]
	public class ProtobufExample : BetterBehaviour
	{
		[Save]
		private int? nullableIntField;

		[Serialize]
		protected Transform[] transformArray;

		public Dictionary<GameObject, Vector3> dictionary;

		public IMyInterface MyInterface { get; set; }

		public MyBaseClass myBase;

		public MyGenChild<int> myGen;

		public List<List<int>> wontSerialize;

		[Serializable]
		public abstract class MyBaseClass
		{
			[Serialize]
			protected int field0;
		}

		[Serializable]
		public class MyChildClass : MyBaseClass
		{
			public int field1;
		}

		[Serializable]
		public class MyGenChild<T>
		{
			public T field3;
		}

		public interface IMyInterface
		{
			float Value { get; set; }
		}

		[Serializable]
		public class MyImplementer : IMyInterface
		{
			[Save]
			private float value;

			public float Value
			{
				get { return value; }
				set { this.value = value; }
			}
		}
	}
}