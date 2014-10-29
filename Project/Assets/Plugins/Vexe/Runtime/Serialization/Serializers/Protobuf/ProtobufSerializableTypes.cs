using System;
using Vexe.Runtime.Types.Examples;

public static class ProtobufSerializableTypes
{
	public static Type[] Types =
	{
		typeof(ITest),
		typeof(ProtobufExample.MyBaseClass),
		typeof(ProtobufExample.IMyInterface),
		typeof(ProtobufExample.MyGenChild<int>),
	};
}