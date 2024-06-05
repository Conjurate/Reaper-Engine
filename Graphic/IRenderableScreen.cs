namespace Reaper;

public interface IRenderableScreen
{
    public int Layer { get; set; }
    public bool IsRenderable(RenderMode mode);
    public void Render(RenderMode mode);
}
