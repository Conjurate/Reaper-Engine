using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Reaper.UI;

public struct ScaledBounds
{
    public BoundingBox Bounds
    {
        get => targetBounds * new Vector2(Screen.Width / targetBounds.Width, Screen.Height / targetBounds.Height);
        set => targetBounds = value;
    }

    private BoundingBox targetBounds;

    public ScaledBounds(float width, float height)
    {
        targetBounds = new BoundingBox(0, 0, width, height);
    }
}
