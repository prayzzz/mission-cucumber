using Assets.Plugins.Editor.Vexe.Drawers.API.Base;

using Vexe.Runtime.Types.GUI;

namespace Assets.Plugins.Editor.Vexe.Drawers.User.Others
{
    public class ColorDuoDrawer : ObjectDrawer<ColorDuo>
    {
        public override void OnGUI()
        {
            this.foldout = this.gui.Foldout(this.niceName, this.foldout);

            if (!this.foldout) return;

            this.gui._beginV();
            {
                this.dmValue.FirstColor = this.gui.ColorField("First", this.dmValue.FirstColor);
                this.dmValue.SecondColor = this.gui.ColorField("Second", this.dmValue.SecondColor);
            }
            this.gui._endV();
        }
    }
}