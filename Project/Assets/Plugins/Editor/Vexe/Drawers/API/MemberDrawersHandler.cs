using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Assets.Plugins.Editor.Vexe.Drawers.API.Base;
using Assets.Plugins.Editor.Vexe.Drawers.API.Core;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;

using UnityEditor;

using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Assets.Plugins.Editor.Vexe.Drawers.API
{
    public class MemberDrawersHandler
    {
        private readonly Type[] objectDrawerTypes;
        private readonly Type[] attributeDrawerTypes;
        private readonly Type[] compositeDrawerTypes;
        private readonly Dictionary<MemberInfo, List<BaseDrawer>> cachedCompositeDrawers;
        private readonly Dictionary<MemberInfo, BaseDrawer> cachedAttributeDrawers;
        private readonly Dictionary<MemberInfo, BaseDrawer> cachedCoreDrawers;
        private readonly Dictionary<MemberInfo, BaseDrawer> cachedMethodDrawers;
        private readonly DrawerResolver[] resolvers;
        private readonly Type fallbackDrawerType;
        private static readonly List<BaseDrawer> Empty;
        public static readonly MemberDrawersHandler Instance;

        static MemberDrawersHandler()
        {
            Empty = new List<BaseDrawer>();
            Instance = new MemberDrawersHandler();
        }

        public MemberDrawersHandler()
        {
            this.cachedAttributeDrawers = new Dictionary<MemberInfo, BaseDrawer>();
            this.cachedCoreDrawers = new Dictionary<MemberInfo, BaseDrawer>();
            this.cachedMethodDrawers = new Dictionary<MemberInfo, BaseDrawer>();
            this.cachedCompositeDrawers = new Dictionary<MemberInfo, List<BaseDrawer>>();

            this.fallbackDrawerType = typeof(RecursiveDrawer<>);

            this.resolvers = new[]
            {
                new AttributeDrawerResolver().Init(this),
                new ObjectDrawerResolver().Init(this)
            };

            Type[] drawerTypes = Assembly.GetExecutingAssembly().GetTypes()
                                                  .Where(t => t.IsA<BaseDrawer>())
                                                  .Where(t => !t.IsAbstract)
                                                  .ToArray();

            this.compositeDrawerTypes = drawerTypes.Where(t => t.IsSubclassOfRawGeneric(typeof(CompositeDrawer<,>)))
                                                         .ToArray();

            this.attributeDrawerTypes = drawerTypes.Where(t => t.IsSubclassOfRawGeneric(typeof(AttributeDrawer<,>)))
                                                         .ToArray();

            this.objectDrawerTypes = drawerTypes.Except(this.attributeDrawerTypes)
                                                     .Disinclude(this.fallbackDrawerType)
                                                     .Where(t => t.IsSubclassOfRawGeneric(typeof(ObjectDrawer<>)))
                                                     .ToArray();
        }

        public List<BaseDrawer> GetCompositeDrawers(MemberInfo member, Attribute[] attributes, bool coreOnly)
        {
            if (coreOnly || member.MemberType == MemberTypes.Method)
                return Empty;

            List<BaseDrawer> drawers;
            if (this.cachedCompositeDrawers.TryGetValue(member, out drawers))
                return drawers;

            drawers = new List<BaseDrawer>();
            var memberType = this.GetMemberType(member);

            // consider composition only if coreOnly was false, and the member type isn't a collection type,
            // or it is a collection type but it doesn't have any per attribute that signifies drawing per element
            // (in other words, the composition is applied on the collection itself, and not its elements)
            var considerComposition = !memberType.IsCollection() || !attributes.AnyDefined<DefinesElementDrawingAttribute>();

            if (considerComposition)
            {
                var compositeAttributes = attributes.Where(a => a is CompositeAttribute)
                                                                .Cast<CompositeAttribute>()
                                                                .OrderBy(x => x.id)
                                                                .ToArray();

                for (int i = 0; i < compositeAttributes.Length; i++)
                    drawers.AddIfNotNull(this.GetCompositeDrawer(memberType, compositeAttributes[i].GetType()));
            }

            this.cachedCompositeDrawers.Add(member, drawers);
            return drawers;
        }

        public BaseDrawer GetMemberDrawer(MemberInfo member, Attribute[] attributes, bool coreOnly)
        {
            if (member.MemberType == MemberTypes.Method)
                return this.GetMethodDrawer(member);

            Dictionary<MemberInfo, BaseDrawer> cache = coreOnly ?
                this.cachedCoreDrawers : this.cachedAttributeDrawers;

            BaseDrawer drawer;
            if (cache.TryGetValue(member, out drawer))
                return drawer;

            var memberType = this.GetMemberType(member);

            for (int i = 0; i < this.resolvers.Length; i++)
                if ((drawer = this.resolvers[i].Resolve(memberType, attributes, coreOnly)) != null)
                    break;

            cache.Add(member, drawer);
            return drawer;
        }

        private BaseDrawer GetMethodDrawer(MemberInfo method)
        {
            BaseDrawer drawer;
            if (!this.cachedMethodDrawers.TryGetValue(method, out drawer))
                this.cachedMethodDrawers.Add(method, drawer = new MethodDrawer());
            return drawer;
        }

        public BaseDrawer GetObjectDrawer(Type objectType, bool coreOnly)
        {
            return this.GetDrawerForType(objectType, coreOnly ?
                    this.objectDrawerTypes.Where(t => t.IsDefined<CoreDrawerAttribute>(true)).ToArray() :
                    this.objectDrawerTypes, typeof(ObjectDrawer<>),
                () => BaseDrawer.Create(this.fallbackDrawerType.MakeGenericType(objectType)));
        }

        public BaseDrawer GetCompositeDrawer(Type objectType, Type attributeType)
        {
            return this.GetDrawerForPair(objectType, attributeType, this.compositeDrawerTypes, typeof(CompositeDrawer<,>));
        }

        public BaseDrawer GetAttributeDrawer(Type objectType, Type attributeType)
        {
            return this.GetDrawerForPair(objectType, attributeType, this.attributeDrawerTypes, typeof(AttributeDrawer<,>));
        }

        private BaseDrawer ResolveDrawerFromTypes(Type objectType, Type drawerType, Type drawerGenArgType)
        {
            if (objectType.IsArray) // TODO: remove concrete dependency
            { 
                return BaseDrawer.Create(typeof(ArrayDrawer<>).MakeGenericType(objectType.GetElementType()));
            }

            if (objectType.IsA(drawerGenArgType))
            {
                return BaseDrawer.Create(drawerType);
            }

            if (drawerGenArgType.IsGenericType)
            {
                if (objectType.IsSubTypeOfRawGeneric(drawerGenArgType.GetGenericTypeDefinition()))
                {
                    return BaseDrawer.Create(drawerType.MakeGenericType(objectType.IsArray ? new[] { objectType.GetElementType() } : objectType.GetGenericArguments()));
                }
            }
            else if (!drawerGenArgType.IsConstructedGenType())
            {
                //if (objectType.IsArray)
                //    return BaseDrawer.Create(drawerType.MakeGenericType(objectType.GetElementType()));
                var args = drawerType.GetGenericArguments();
                if (args.Length == 1 && args[0].IsGenericTypeDefinition)
                    return BaseDrawer.Create(drawerType.MakeGenericType(objectType));
            }

            return null;
        }

        private BaseDrawer GetDrawerForType(Type objectType, Type[] typeCache, Type baseDrawerType, Func<BaseDrawer> fallback)
        {
            for (int i = 0; i < typeCache.Length; i++)
            {
                var drawerType = typeCache[i];
                var firstGen = drawerType.GetParentGenericArguments(baseDrawerType)[0];
                var drawer = this.ResolveDrawerFromTypes(objectType, drawerType, firstGen);
                if (drawer != null)
                {
                    return drawer;
                }
            }

            return fallback();
        }

        private BaseDrawer GetDrawerForPair(Type objectType, Type attributeType, Type[] typeCache, Type baseDrawerType)
        {
            for (int i = 0; i < typeCache.Length; i++)
            {
                var drawerType = typeCache[i];
                var args = drawerType.GetParentGenericArguments(baseDrawerType);

                if (attributeType == args[1])
                {
                    var drawer = this.ResolveDrawerFromTypes(objectType, drawerType, args[0]);
                    if (drawer != null)
                    {
                        return drawer;
                    }
                }
                else if (args[1].IsGenericParameter)
                {
                    var constraints = args[1].GetGenericParameterConstraints();
                    if (constraints.Length == 1 && attributeType.IsA(constraints[0]))
                    {
                        return BaseDrawer.Create(drawerType.MakeGenericType(attributeType));
                    }
                }
            }

            return null;
        }

        private Type GetMemberType(MemberInfo m)
        {
            return m.GetDataType(DataInfo.Fallback);
        }

        private abstract class DrawerResolver
        {
            protected MemberDrawersHandler handler;

            public DrawerResolver Init(MemberDrawersHandler handler)
            {
                this.handler = handler;
                return this;
            }

            public abstract BaseDrawer Resolve(Type memberType, IEnumerable<Attribute> attributes, bool coreOnly);
        }

        private class AttributeDrawerResolver : DrawerResolver
        {
            public override BaseDrawer Resolve(Type memberType, IEnumerable<Attribute> attributes, bool coreOnly)
            {
                if (coreOnly) return null;
                var drawingAttribute = attributes.FirstOrDefault(a => a is DrawnAttribute);
                if (drawingAttribute == null) return null;
                return this.handler.GetAttributeDrawer(memberType, drawingAttribute.GetType());
            }
        }

        private class ObjectDrawerResolver : DrawerResolver
        {
            public override BaseDrawer Resolve(Type memberType, IEnumerable<Attribute> attributes, bool coreOnly)
            {
                return this.handler.GetObjectDrawer(memberType, coreOnly);
            }
        }

        private struct TypePair
        {
            public readonly Type type1;
            public readonly Type type2;

            public TypePair(Type type1, Type type2)
            {
                this.type1 = type1;
                this.type2 = type2;
            }

            public override bool Equals(object obj)
            {
                try
                {
                    var pair = (TypePair)obj;
                    return pair.type1 == this.type1 && pair.type2 == this.type2;
                }
                catch
                {
                    return false;
                }
            }

            public override int GetHashCode()
            {
                return this.type1.GetHashCode() + this.type2.GetHashCode();
            }
        }

        private static class MenuItems
        {
            [MenuItem("Tools/Vexe/BetterBehaviour/Clear drawers cache")]
            public static void ClearCache()
            {
                MemberDrawersHandler.Instance.cachedAttributeDrawers.Clear();
            }
        }
    }
}