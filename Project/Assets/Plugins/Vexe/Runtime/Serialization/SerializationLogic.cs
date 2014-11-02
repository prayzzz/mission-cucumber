using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Fasterflect;

using UnityEngine;

using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

using Object = UnityEngine.Object;

namespace Assets.Plugins.Vexe.Runtime.Serialization
{
    public class SerializationLogic
    {
        protected readonly SerializationAttributes attributes;

        private static SerializationLogic defaultLogic;
        public static SerializationLogic Default
        {
            get { return defaultLogic ?? (defaultLogic = new SerializationLogic(SerializationAttributes.Default)); }
        }

        public SerializationAttributes Attributes { get { return this.attributes; } }

        public SerializationLogic(SerializationAttributes attributes)
        {
            this.attributes = attributes;
        }

        public IEnumerable<DataMember> GetSerializableMembers(object target)
        {
            return this.GetSerializableMembers(target.GetType(), target);
        }

        public IEnumerable<DataMember> GetSerializableMembers(Type type, object target)
        {
            return this.GetSerializableMembers(type, target, Flags.InstanceAnyVisibility);
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

            return DataMember.Enumerate(members, target).Where(this.IsSerializableMember);
        }

        /// <summary>
        /// A member is visible if it was serializable or had any of the exposure attributes defined on it
        /// </summary>
        public bool IsVisibleMember(MemberInfo member)
        {
            if (member is MethodInfo)
                return this.attributes.Exposure.Any(member.IsDefined);

            var field = member as FieldInfo;
            if (field != null)
                return !this.attributes.Hide.Any(field.IsDefined)
                    && (this.IsSerializable(field) || this.attributes.Exposure.Any(field.IsDefined));

            var property = member as PropertyInfo;
            return property != null
                && !this.attributes.Hide.Any(property.IsDefined)
                && (this.IsSerializable(property) || this.attributes.Exposure.Any(property.IsDefined));
        }

        public bool IsSerializableMember<T>(DataMember<T> member)
        {
            var field = (FieldInfo)member;
            return field != null ? this.IsSerializable(field) : this.IsSerializable((PropertyInfo)member);
        }

        public bool IsSerializable(Type type)
        {
            if (IsSimpleType(type)
                || type.IsA<Object>()
                || UnityStructs.ContainsValue(type))
                return true;

            if (type.IsArray)
                return type.GetArrayRank() == 1 && this.IsSerializable(type.GetElementType());

            if (type.IsInterface)
                return true;

            if (NonSupportedTypes.Any(type.IsA))
                return false;

            if (SupportedTypes.Any(type.IsA))
                return true;

            if (type.IsGenericType)
                return type.GetGenericArguments().All(this.IsSerializable);

            return this.attributes.SerializableType.IsEmpty() || this.attributes.SerializableType.Any(type.IsDefined);
        }

        public bool IsSerializable(FieldInfo field)
        {
            if (this.attributes.DontSerializeMember.Any(field.IsDefined))
                return false;

            Type fieldType = field.FieldType;

            return!field.IsLiteral
                && !field.IsStatic
                && (field.IsPublic || this.attributes.SerializeMember.Any(field.IsDefined))
                && this.IsSerializable(fieldType);
        }

        public bool IsSerializable(PropertyInfo property)
        {
            return!this.attributes.DontSerializeMember.Any(property.IsDefined)
                 && property.IsAutoProperty()
                 && ((property.GetGetMethod(true).IsPublic && property.GetSetMethod(true).IsPublic)
                 || this.attributes.SerializeMember.Any(property.IsDefined))
                 && this.IsSerializable(property.PropertyType);
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