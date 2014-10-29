using System;
using UnityEditor;
using UnityEngine;
using Vexe.Editor.Framework.GUIs;
using Vexe.Editor.Helpers;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;
using Random = UnityEngine.Random;

namespace Vexe.Editor.Framework.Drawers
{
	public abstract class BetterVectorDrawer<TVector> : AttributeDrawer<TVector, BetterVectorAttribute>
	{
		private const string kBtnReset     = "0";
		private const string kBtnNormalize = "1";
		private const string kBtnRandomize = "r";
		protected static Func<float> rand  = () => Random.Range(-100, 100);

		public override void OnGUI()
		{
			DoWideTrick(DoField, DoFoldout, DoButtons);
		}

		private void DoField()
		{
			var current = dmValue;
			var newValue = GetField()(niceName, current);
			{
				if (!VectorEquals(current, newValue))
				{
					dmValue = newValue;
				}
			}
		}

		private void DoFoldout()
		{
			gui.Space(7f);
			Foldout();
		}

		protected abstract Func<string, TVector, TVector> GetField();
		protected abstract bool VectorEquals(TVector left, TVector right);

		private void DoButtons()
		{
			if (foldout)
			{
				gui.FlexibleSpace();
				var option = GUILayout.Height(13f);
				if (gui.MiniButton("Copy", "Copy vector value", option))
					Copy();
				if (gui.MiniButton("Paste", "Paste vector value", option))
					dmValue = Paste();
				if (gui.MiniButton(kBtnRandomize, "Randomize values between [-100, 100]", MiniButtonStyle.ModMid, option))
					dmValue = Randomize();
				if (gui.MiniButton(kBtnNormalize, "Normalize", MiniButtonStyle.ModMid, option))
					dmValue = Normalize();
				if (gui.MiniButton(kBtnReset, "Reset", MiniButtonStyle.ModRight, option))
					dmValue = Reset();
			}
		}

		protected abstract TVector Reset();
		protected abstract TVector Normalize();
		protected abstract TVector Randomize();
		protected abstract TVector Paste();
		protected abstract void Copy();

		// Just to play nice with wideMode being false
		private void DoWideTrick(Action field, Action doFoldout, Action buttons)
		{
			if (EditorGUIUtility.wideMode)
			{
				gui._beginH();
				{
					field();
					doFoldout();
				}
				gui._endH();

				gui._beginH();
				{
					buttons();
					gui.Space(25f);
				}
				gui._endH();
			}
			else
			{
				// draw the field on its own
				gui._beginH();
				{
					field();
				}
				gui._endH();

				// and then the buttons and the foldout, on their own - pull them up to allign them
				gui.Space(-35);

				gui._beginH();
				{
					gui.FlexibleSpace();
					buttons();
					doFoldout();
				}
				gui._endH();

				// finally some space so we don't overlap on neighbour controls
				gui.Space(18f);
			}
		}
	}

	public class BetterVector2Drawer : BetterVectorDrawer<Vector2>
	{
		protected override void Copy() { Clipboard.Vector2Value = dmValue; }
		protected override Vector2 Paste() { return Clipboard.Vector2Value; }
		protected override Vector2 Randomize() { return new Vector2(rand(), rand()); }
		protected override Vector2 Normalize() { return Vector2.one; }
		protected override Vector2 Reset() { return Vector2.zero; }

		protected override Func<string, Vector2, Vector2> GetField()
		{
			return gui.Vector2Field;
		}

		protected override bool VectorEquals(Vector2 left, Vector2 right)
		{
			return left.ApproxEqual(right);
		}
	}

	public class BetterVector3Drawer : BetterVectorDrawer<Vector3>
	{
		protected override void Copy() { Clipboard.Vector3Value = dmValue; }
		protected override Vector3 Paste() { return Clipboard.Vector3Value; }
		protected override Vector3 Randomize() { return new Vector3(rand(), rand(), rand()); }
		protected override Vector3 Normalize() { return Vector3.one; }
		protected override Vector3 Reset() { return Vector3.zero; }

		protected override Func<string, Vector3, Vector3> GetField()
		{
			return gui.Vector3Field;
		}

		protected override bool VectorEquals(Vector3 left, Vector3 right)
		{
			return left.ApproxEqual(right);
		}
	}
}