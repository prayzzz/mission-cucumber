using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fasterflect;
using UnityEngine;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;
using UnityObject = UnityEngine.Object;

namespace Vexe.Runtime.Serialization
{
	public class SerializationLogic
	{
		protected readonly SerializationAttributes attributes;

		private static SerializationLogic defaultLogic;
		public static SerializationLogic Default
		{
			get { return defaultLogic ?? (defaultLogic = new SerializationLogic(SerializationAttributes.Default)); }
		}

		public SerializationAttributes Attributes { get { return attributes; } }

		public SerializationLogic(SerializationAttributes attributes)
		{
			this.attributes = attributes;
		}

		public IEnumerable<DataMember> GetSerializableMembers(object target)
		{
			return GetSerializableMembers(target.GetType(), target);
		}

		public IEnumerable<DataMember> GetSerializableMembers(Type type, object target)
		{
			return GetSerializableMembers(type, target, Flags.InstanceAnyVisibility);
		}

		public IEnumerable<DataMember> GetSerializableMembers(Type type, object target, BindingFlags flags)
		{
			List<MemberInfo> members = null;
			for (int i = 0; i < ShouldGetBeneath.Length; i++)
			{
				var beneath = ShouldGetBeneath[i];
				if (type.IsA(beneath))
				{
					members = type.GetMembersBeneath(beneath).ToList();
					break;
				}
			}

			if (members == null)
				members = type.GetMembers(flags).ToList();

			return DataMember.Enumerate(members, target).Where(IsSerializableMember);
		}

		/// <summary>
		/// A member is visible if it was serializable or had any of the exposure attributes defined on it
		/// </summary>
		public bool IsVisibleMember(MemberInfo member)
		{
			if (member is MethodInfo)
				return attributes.Exposure.Any(member.IsDefined);

			var field = member as FieldInfo;
			if (field != null)
				return !attributes.Hide.Any(field.IsDefined)
					&& (IsSerializable(field) || attributes.Exposure.Any(field.IsDefined));

			var property = member as PropertyInfo;
			return property != null
				&& !attributes.Hide.Any(property.IsDefined)
				&& (IsSerializable(property) || attributes.Exposure.Any(property.IsDefined));
		}

		public bool IsSerializableMember<T>(DataMember<T> member)
		{
			var field = (FieldInfo)member;
			return field != null ? IsSerializable(field) : IsSerializable((PropertyInfo)member);
		}

		public bool IsSerializable(Type type)
		{
			if (IsSimpleType(type)
				|| type.IsA<UnityObject>()
				|| UnityStructs.ContainsValue(type))
				return true;

			if (type.IsArray)
				return type.GetArrayRank() == 1 && IsSerializable(type.GetElementType());

			if (type.IsInterface)
				return true;

			if (NonSupportedTypes.Any(type.IsA))
				return false;

			if (SupportedTypes.Any(type.IsA))
				return true;

			if (type.IsGenericType)
				return type.GetGenericArguments().All(IsSerializable);

			return attributes.SerializableType.IsEmpty() || attributes.SerializableType.Any(type.IsDefined);
		}

		public bool IsSerializable(FieldInfo field)
		{
			if (attributes.DontSerializeMember.Any(field.IsDefined))
				return false;

			Type fieldType = field.FieldType;

			return!field.IsLiteral
				&& !field.IsStatic
				&& (field.IsPublic || attributes.SerializeMember.Any(field.IsDefined))
				&& IsSerializable(fieldType);
		}

		public bool IsSerializable(PropertyInfo property)
		{
			return!attributes.DontSerializeMember.Any(property.IsDefined)
				 && property.IsAutoProperty()
				 && ((property.GetGetMethod(true).IsPublic && property.GetSetMethod(true).IsPublic)
				 || attributes.SerializeMember.Any(property.IsDefined))
				 && IsSerializable(property.PropertyType);
		}

		public static readonly Type[] UnityStructs =
		{
			typeof(Vector3),
			typeof(Vector2),
			typeof(Vector4),
			typeof(Rect),
			typeof(Quaternion),
			typeof(Matrix4x4),
			typeof(Color),
			typeof(Color32),
			typeof(LayerMask),
			typeof(Bounds)
		};

		public static readonly Type[] NonSupportedTypes =
		{
			typeof(Delegate)
		};

		public static readonly Type[] SupportedTypes =
		{
			typeof(Type)
		};

		public static readonly Type[] ShouldGetBeneath =
		{
			typeof(MonoBehaviour),
		};

		private static bool IsSimpleType(Type type)
		{
			return type.IsPrimitive || type.IsEnum || type == typeof(string);
		}
	}
}