using System.Numerics;

namespace Reaper;

public class QuadTree<T>
{
    public Node Root => root;

    private Node root;
    private int maxItems;

    public QuadTree(BoundingBox bounds, int maxItems = 16)
    {
        root = new Node(bounds);
        this.maxItems = maxItems;
    }

    public void Insert(T item, Vector2 pos)
    {
        Insert(root, item, pos);
    }

    private void Insert(Node node, T item, Vector2 pos)
    {
        if (!node.Contains(pos))
            return;

        if (node.IsLeaf)
        {
            node.items.Add(new NodeItem(item, pos));

            if (node.items.Count > maxItems)
            {
                Split(node);
            }
        }
        else
        {
            foreach (Node child in node.children)
            {
                Insert(child, item, pos);
            }
        }
    }

    private void Split(Node node)
    {
        //int childCount = (int)Math.Ceiling(Math.Sqrt(maxItems));
        float width = node.bounds.Size.X * 0.5f;
        float height = node.bounds.Size.Y * 0.5f;

        node.children = new Node[4];

        for (int x = 0; x < 2; x++)
        {
            for (int y = 0; y < 2; y++)
            {
                // Calculate the center of the child node
                float childCenterX = node.bounds.Min.X + (x + 0.5f) * width;
                float childCenterY = node.bounds.Min.Y + (y + 0.5f) * height;
                BoundingBox bounds = new BoundingBox(new Vector2(childCenterX, childCenterY), new Vector2(width, height));
                node.children[x * 2 + y] = new Node(bounds);
            }
        }

        foreach (NodeItem nodeItem in node.items)
        {
            foreach (Node child in node.children)
            {
                Insert(child, nodeItem.item, nodeItem.pos);
            }
        }

        node.items.Clear();
    }

    public List<T> Query(Vector2 pos)
    {
        List<T> items = new List<T>();
        Query(root, pos, items);
        return items;
    }

    private void Query(Node node, Vector2 pos, List<T> result)
    {
        if (!node.IsLeaf && !node.Contains(pos))
            return;

        if (node.IsLeaf)
        {
            foreach (NodeItem nodeItem in node.items)
                result.Add(nodeItem.item);
            return;
        }

        foreach (var child in node.children)
        {
            if (child.Contains(pos))
                Query(child, pos, result);
        }
    }

    public List<T> Query(Vector2 pos, float dist)
    {
        List<T> items = new List<T>();
        Query(root, pos, dist, items);
        return items;
    }

    private void Query(Node node, Vector2 pos, float dist, List<T> result)
    {
        if (!node.Intersects(new BoundingBox(pos, new Vector2(dist * 2, dist * 2))))
            return;

        if (node.IsLeaf)
        {
            foreach (NodeItem nodeItem in node.items)
            {
                if (Vector2.Distance(nodeItem.pos, pos) <= dist)
                {
                    result.Add(nodeItem.item);
                }
            }
        }
        else
        {
            foreach (Node child in node.children)
            {
                Query(child, pos, dist, result);
            }
        }
    }

    public List<T> Query(BoundingBox bounds)
    {
        List<T> result = new List<T>();
        Query(root, bounds, result);
        return result;
    }

    private void Query(Node node, BoundingBox bounds, List<T> result)
    {
        if (!node.Intersects(bounds))
            return;

        if (node.IsLeaf)
        {
            foreach (NodeItem nodeItem in node.items)
            {
                result.Add(nodeItem.item);
            }
        }
        else
        {
            foreach (var child in node.children)
            {
                Query(child, bounds, result);
            }
        }
    }

    public class Node
    {
        public bool IsLeaf => children == null;

        public BoundingBox bounds;
        public List<NodeItem> items = new List<NodeItem>();
        public Node[] children;

        public Node(BoundingBox bounds)
        {
            this.bounds = bounds;
        }

        public bool Contains(Vector2 pos) => bounds.Contains(pos);

        public bool Intersects(BoundingBox bounds) => this.bounds.Intersects(bounds);
    }

    public class NodeItem
    {
        public T item;
        public Vector2 pos;

        public NodeItem(T item, Vector2 pos)
        {
            this.item = item;
            this.pos = pos;
        }
    }
}
