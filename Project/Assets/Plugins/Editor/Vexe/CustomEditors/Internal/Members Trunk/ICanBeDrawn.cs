namespace Assets.Plugins.Editor.Vexe.CustomEditors.Internal
{
    public interface ICanBeDrawn
    {
        float DisplayOrder { get; set; }
        string Name { get; }
        void Draw();
        void HeaderSpace();
    }
}