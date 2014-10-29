using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using UnityEngine;
using Vexe.Runtime.Exceptions;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;
using UnityObject = UnityEngine.Object;

namespace Vexe.Runtime.Serialization.Serializers
{
	public abstract class BaseSerializer
	{
		public SerializationLogic Logic { protected set; get; }

		public static BaseSerializer Create(Type serializerType)
		{
			if (!serializerType.IsA<BaseSerializer>())
				throw new TypeMismatchException(string.Format("Serializer `{0}` must be a BetterSerializer derivative", serializerType));

			return serializerType.Instance<BaseSerializer>().Initialize();
		}

		public BaseSerializer Initialize()
		{
			InitializeLogic();
			InternalInitialize();

			var serializerType = GetType();
			var userAsm = serializerType.Assembly;

			(from surrogate in userAsm.GetTypes()
			 let attribute = surrogate.GetCustomAttribute<SurrogateForAttribute>()
			 where attribute != null
			 where attribute.forSerializer == serializerType
			 select new { type = attribute.forType, surrogate })
			 .Foreach(x => AddTypeSurrogate(x.type, x.surrogate));

			Func<Assembly, IEnumerable<Type>> getTypes = asm =>
				asm.GetTypes().Where(t => t.IsA<UnityObject>());

			getTypes(typeof(UnityObject).Assembly).Concat(getTypes(userAsm))
												 .Foreach(AddUnityTypeSurrogate);

			return this;
		}

		protected virtual void InitializeLogic()
		{
			Logic = SerializationLogic.Default;
		}

		public void Save(object target, SerializationData data)
		{
			var members = Logic.GetSerializableMembers(target).ToList();
			for (int i = 0; i < members.Count; i++)
			{
				var member = members[i];
				var value  = member.Value;

				if (value.IsObjectNull())
					continue;

				try
				{
					string memberKey = GetMemberKey(member);
					string serializedState = Serialize(member.Type, value, data.serializedObjects);
					data.serializedStrings[memberKey] = serializedState;
				}
				catch (Exception e)
				{
					Debug.LogError("Serialization error: " + e.Message + " | " + e.StackTrace);
				}
			}
		}

		public void Load(object target, SerializationData data)
		{
			var members = Logic.GetSerializableMembers(target).ToList();
			for(int i = 0; i < members.Count; i++)
			{
				var member    = members[i];
				var value     = member.Type.GetDefaultValue();
				var memberKey = GetMemberKey(member);

				try
				{
					string result;
					if (data.serializedStrings.TryGetValue(memberKey, out result))
						value = Deserialize(member.Type, result, data.serializedObjects);
				}
				catch (Exception e)
				{
					Debug.LogError("Deserialization error: " + e.Message + " | " + e.StackTrace);
				}

				member.Value = value;
			}
		}

		private string GetMemberKey(DataMember member)
		{
			return string.Format("{0}: {1} {2}", member.MemberType.ToString(), member.Type.FullName, member.Name);
		}

		public abstract string Serialize(Type type, object graph, object context);
		public string Serialize<T>(T graph, object context)
		{
			return Serialize(typeof(T), graph, context);
		}
		public string Serialize<T>(T graph)
		{
			return Serialize(graph, null);
		}
		public abstract object Deserialize(Type type, string serializedState, object context);
		protected abstract void InternalInitialize();
		protected abstract void AddTypeSurrogate(Type type, Type surrogate);
		protected abstract void AddUnityTypeSurrogate(Type type);
	}
}