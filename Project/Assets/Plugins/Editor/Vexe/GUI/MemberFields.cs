#define PROFILE

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Vexe.Editor.Framework.Drawers;
using Vexe.Editor.Helpers;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;
using UnityObject = UnityEngine.Object;

namespace Vexe.Editor.Framework.GUIs
{
	public partial class GLWrapper
	{
		public static class Cache
		{
			private static Func<MemberInfo, Attribute[]> _getAttributes;
			public static Func<MemberInfo, Attribute[]> GetAttributes
			{
				get { return _getAttributes ?? (_getAttributes = new Func<MemberInfo, Attribute[]>(member => member.GetAttributes()).Memoize()); }
			}
		}

		public bool MemberField<T>(DataMember<T> member, UnityObject unityTarget, string key = null, bool coreOnly = true)
		{
			return MemberField(member, member.Attributes, unityTarget, key, coreOnly);
		}

		public bool MemberField<T>(DataMember<T> member, Attribute[] attributes, UnityObject unityTarget, string key = null, bool coreOnly = true)
		{
			return MemberField(member, attributes, member.Target, unityTarget, key, coreOnly);
		}

		public bool MemberField(MemberInfo member, object rawTarget, UnityObject unityTarget, string key = null, bool coreOnly = true)
		{
			return MemberField(member, Cache.GetAttributes(member), rawTarget, unityTarget, key, coreOnly);
		}

		public bool MemberField(MemberInfo member, Attribute[] attributes, object rawTarget, UnityObject unityTarget, string key = null, bool coreOnly = true)
		{
			#if PROFILE
			Profiler.BeginSample("Fetching&InitingDrawer for " + member.Name);
			#endif
			var handler      = MemberDrawersHandler.Instance;
			var composites   = handler.GetCompositeDrawers(member, attributes, coreOnly);
			var memberDrawer = handler.GetMemberDrawer(member, attributes, coreOnly);

			for (int i = 0; i < composites.Count; i++)
			{
				composites[i].Initialize(member, attributes, rawTarget, unityTarget, this, key);
			}
			memberDrawer.Initialize(member, attributes, rawTarget, unityTarget, this, key);
			#if PROFILE
			Profiler.EndSample();
			#endif

			if (composites.Count == 0)
			{
				#if PROFILE
				Profiler.BeginSample(memberDrawer.GetType().Name + " OnGUI (C)");
				#endif
				_beginChange();
				{
					memberDrawer.OnGUI();
				}
				#if PROFILE
				Profiler.EndSample();
				#endif
				return _endChange();
			}

			Action<List<BaseDrawer>, Action<BaseDrawer>> drawSection = (drawers, section) =>
			{
				for (int i = 0; i < drawers.Count; i++)
					section(drawers[i]);
			};

			bool changed = false;

			#if PROFILE
			Profiler.BeginSample("OnUpperGUI " + member.Name);
			#endif
			drawSection(composites, d => d.OnUpperGUI());
			#if PROFILE
			Profiler.EndSample();
			#endif
			_beginH();
			{
				#if PROFILE
				Profiler.BeginSample("OnLeftGUI " + member.Name);
				#endif
				drawSection(composites, d => d.OnLeftGUI());
				#if PROFILE
				Profiler.EndSample();
				#endif
				_beginV();
				{
					#if PROFILE
					Profiler.BeginSample(memberDrawer.GetType().Name + " OnGUI");
					#endif
					_beginChange();
					{
						memberDrawer.OnGUI();
					}
					#if PROFILE
					Profiler.EndSample();
					#endif
					if (_endChange())
						changed = true;

					#if PROFILE
					Profiler.BeginSample("OnMemberDrawn" + member.Name);
					#endif
					drawSection(composites, d => d.OnMemberDrawn(GUIHelper.GetLastRect()));
					#if PROFILE
					Profiler.EndSample();
					#endif
				}
				_endV();

				#if PROFILE
				Profiler.BeginSample("OnRightGUI " + member.Name);
				#endif
				drawSection(composites, d => d.OnRightGUI());
				#if PROFILE
				Profiler.EndSample();
				#endif
			}
			_endH();
			#if PROFILE
			Profiler.BeginSample("OnLowerGUI " + member.Name);
			#endif
			drawSection(composites, d => d.OnLowerGUI());
			#if PROFILE
			Profiler.EndSample();
			#endif
			return changed;
		}
	}
}