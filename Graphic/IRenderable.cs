namespace Reaper;

public interface IRenderable
{
    public int Layer { get; set; }
    public bool IsRenderable(RenderMode mode);
    public void Render(RenderMode mode);
}
