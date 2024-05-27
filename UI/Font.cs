using Raylib_cs;

namespace Reaper.UI;

public struct Font
{
    internal Raylib_cs.Font raylibFont;

    internal Font(Raylib_cs.Font raylibFont)
    {
        this.raylibFont = raylibFont;
    }
}
