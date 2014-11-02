using System;

using Assets.Plugins.Editor.Vexe.Drawers.API.Base;
using Assets.Plugins.Editor.Vexe.GUI;
using Assets.Plugins.Editor.Vexe.Other;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Vectors;

using UnityEditor;

using UnityEngine;

using Vexe.Runtime.Extensions;

using Random = UnityEngine.Random;

namespace Assets.Plugins.Editor.Vexe.Drawers.User.Vectors
{
    public abstract class BetterVectorDrawer<TVector> : AttributeDrawer<TVector, BetterVectorAttribute>
    {
        private const string kBtnReset     = "0";
        private const string kBtnNormalize = "1";
        private const string kBtnRandomize = "r";
        protected static Func<float> rand  = () => Random.Range(-100, 100);

        public override void OnGUI()
        {
            this.DoWideTrick(this.DoField, this.DoFoldout, this.DoButtons);
        }

        private void DoField()
        {
            var current = this.dmValue;
            var newValue = this.GetField()(this.niceName, current);
            {
                if (!this.VectorEquals(current, newValue))
                {
                    this.dmValue = newValue;
                }
            }
        }

        private void DoFoldout()
        {
            this.gui.Space(7f);
            this.Foldout();
        }

        protected abstract Func<string, TVector, TVector> GetField();
        protected abstract bool VectorEquals(TVector left, TVector right);

        private void DoButtons()
        {
            if (this.foldout)
            {
                this.gui.FlexibleSpace();
                var option = GUILayout.Height(13f);
                if (this.gui.MiniButton("Copy", "Copy vector value", option))
                    this.Copy();
                if (this.gui.MiniButton("Paste", "Paste vector value", option))
                    this.dmValue = this.Paste();
                if (this.gui.MiniButton(kBtnRandomize, "Randomize values between [-100, 100]", MiniButtonStyle.ModMid, option))
                    this.dmValue = this.Randomize();
                if (this.gui.MiniButton(kBtnNormalize, "Normalize", MiniButtonStyle.ModMid, option))
                    this.dmValue = this.Normalize();
                if (this.gui.MiniButton(kBtnReset, "Reset", MiniButtonStyle.ModRight, option))
                    this.dmValue = this.Reset();
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
                this.gui._beginH();
                {
                    field();
                    doFoldout();
                }
                this.gui._endH();

                this.gui._beginH();
                {
                    buttons();
                    this.gui.Space(25f);
                }
                this.gui._endH();
            }
            else
            {
                // draw the field on its own
                this.gui._beginH();
                {
                    field();
                }
                this.gui._endH();

                // and then the buttons and the foldout, on their own - pull them up to allign them
                this.gui.Space(-35);

                this.gui._beginH();
                {
                    this.gui.FlexibleSpace();
                    buttons();
                    doFoldout();
                }
                this.gui._endH();

                // finally some space so we don't overlap on neighbour controls
                this.gui.Space(18f);
            }
        }
    }

    public class BetterVector2Drawer : BetterVectorDrawer<Vector2>
    {
        protected override void Copy() { Clipboard.Vector2Value = this.dmValue; }
        protected override Vector2 Paste() { return Clipboard.Vector2Value; }
        protected override Vector2 Randomize() { return new Vector2(rand(), rand()); }
        protected override Vector2 Normalize() { return Vector2.one; }
        protected override Vector2 Reset() { return Vector2.zero; }

        protected override Func<string, Vector2, Vector2> GetField()
        {
            return this.gui.Vector2Field;
        }

        protected override bool VectorEquals(Vector2 left, Vector2 right)
        {
            return left.ApproxEqual(right);
        }
    }

    public class BetterVector3Drawer : BetterVectorDrawer<Vector3>
    {
        protected override void Copy() { Clipboard.Vector3Value = this.dmValue; }
        protected override Vector3 Paste() { return Clipboard.Vector3Value; }
        protected override Vector3 Randomize() { return new Vector3(rand(), rand(), rand()); }
        protected override Vector3 Normalize() { return Vector3.one; }
        protected override Vector3 Reset() { return Vector3.zero; }

        protected override Func<string, Vector3, Vector3> GetField()
        {
            return this.gui.Vector3Field;
        }

        protected override bool VectorEquals(Vector3 left, Vector3 right)
        {
            return left.ApproxEqual(right);
        }
    }
}