namespace Reaper;

public struct Model
{
    internal Raylib_cs.Model raylibModel;

    internal Model(Raylib_cs.Model model)
    {
        raylibModel = model;
    }
}
