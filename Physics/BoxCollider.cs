using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Reaper.Physics;

public class BoxCollider : EntityModule // 1 (0) 2 (-0.5) 3 (-1) U does offset and size only
{
    public BoundingBox Bounds { get; set; }
    public bool Static { get; set; }
}
