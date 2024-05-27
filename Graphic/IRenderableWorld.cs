namespace Reaper;

public interface IRenderableWorld
{
    public int WorldLayer { get; set; }
    public bool IsRenderable(RenderMode mode);
    public void Render(RenderMode mode);
}
