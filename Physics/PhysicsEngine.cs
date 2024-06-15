using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Reaper.Physics;

internal class PhysicsEngine
{
    private Grid entities = new Grid();

    public void AddColliders(Entity entity)
    {
        if (entity.colliders != null)
        {
            entities.Add(entity);
            Log.Debug($"Registered entity {entity.Name} to physics engine");
        }
        /*List<BoxCollider> entityColliders = entity.GetModules<BoxCollider>();
        foreach (BoxCollider collider in entityColliders) 
        {
            colliders.Add(collider);
        }*/

        //Log.Debug($"Registered {entityColliders.Count} colliders from entity {entity.Name}");
    }

    public void RemoveColliders(Entity entity)
    {
        if (entity.colliders != null)
        {
            entities.Remove(entity);
            Log.Debug($"Unregistered entity {entity.Name} from physics engine");
        }
        /*int removed = 0;
        List<BoxCollider> entityColliders = entity.GetModules<BoxCollider>();
        foreach (BoxCollider collider in entityColliders)
        {
            if (colliders.Remove(collider))
                removed++;
        }

        Log.Debug($"Unregistered {removed} colliders from entity {entity.Name}");*/
    }

    public void Update()
    {
        ResolveCollisions();
        entities.ProcessRemoval();
    }

    private void ResolveCollisions()
    {
        foreach (Entity me in entities)
        {
            if (me.colliders == null)
                continue;

            foreach (Entity other in entities.QueryEntities(me.Transform.Position, Engine.CellSize * 2))
            {
                if (other == me || other.colliders == null)
                    continue;

                foreach (BoxCollider colliderA in me.colliders)
                {
                    foreach (BoxCollider colliderB in other.colliders)
                    {
                        // Resolve collision
                        if (colliderA.Static && colliderB.Static)
                            continue;
                        else if (colliderB.Static)
                            Resolve(colliderA, colliderB);
                        else
                            Resolve(colliderB, colliderA);
                    }
                }
            }
        }
        /*for (int i = 0; i < colliders.Count; i++)
        {
            for (int j = i + 1; j < colliders.Count; j++)
            {
                BoxCollider colliderA = colliders[i];
                BoxCollider colliderB = colliders[j];

                // Resolve collision
                Resolve(colliderA, colliderB);
            }
        }*/
    }

    private bool Resolve(BoxCollider boxA, BoxCollider boxB)
    {
        BoundingBox a = boxA.Bounds + boxA.Owner.Transform.Position;
        BoundingBox b = boxB.Bounds + boxB.Owner.Transform.Position;

        if (!a.Intersects(b))
            return false;

        // Calculate overlap on each axis
        float overlapX = Math.Min(a.Right - b.Left, b.Right - a.Left);
        float overlapY = Math.Min(a.Top - b.Bottom, b.Top - a.Bottom);

        // Resolve collision in the direction of least overlap
        Vector2 adjustment;
        if (overlapX < overlapY)
        {
            adjustment = new Vector2 (a.Left < b.Left ? -overlapX : overlapX, 0);
        }
        else
        {
            adjustment = new Vector2(0, a.Bottom < b.Bottom ? -overlapY : overlapY);
        }

        boxA.Owner.Transform.Position += adjustment;

        return true;
    }
}

internal struct Manifold
{
    BoxCollider a;
    BoxCollider b;
    float penetration;
    Vector2 normal;
}
