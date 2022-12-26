namespace AoC_2022;

public interface IDijkstraNode<TNode>
    where TNode : IDijkstraNode<TNode>
{
    public IList<TNode> Children { get; }
}

public static class Helpers
{
    public static Dictionary<TNode, int> Dijkstra<TNode>(List<TNode> input, TNode start, TNode? end = default)
        where TNode : IDijkstraNode<TNode>
    {
        PriorityQueue<TNode, int> priorityQueue = new(input.Count);
        Dictionary<TNode, TNode?> previousNode = new(input.Count);
        Dictionary<TNode, int> distanceToSource = new(input.Count)
        {
            [start] = 0
        };
        const int maxDistance = 1_000_000_000;  // safe value that can be safely increased with node-neighbour distance

        foreach (var point in input)
        {
            if (!point.Equals(start))
            {
                distanceToSource[point] = maxDistance;
                previousNode[point] = default;
            }

            priorityQueue.Enqueue(point, distanceToSource[point]);
        }

        while (priorityQueue.TryDequeue(out var node, out var priority))
        {
            foreach (var neighbour in node.Children)
            {
                var distance = priority + 1;    // Distance between source and node + distance between neighbourd and node
                if (distance < distanceToSource[neighbour])
                {
                    distanceToSource[neighbour] = distance;
                    priorityQueue.Enqueue(neighbour, distance);
                    previousNode[neighbour] = node;

                    if (neighbour.Equals(end))
                    {
                        return distanceToSource;
                    }
                }
            }
        }

        return distanceToSource;
    }

}
