using System.Collections.Generic;

namespace FuzzySearch;

internal class NodeCollection
{
    private readonly List<Node> _nodes = new();

    internal int Add(Node node)
    {
        _nodes.Add(node);
        return _nodes.Count - 1;
    }

    internal Node this[int index] => _nodes[index];
}