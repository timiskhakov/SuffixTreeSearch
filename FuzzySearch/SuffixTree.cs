namespace FuzzySearch;

public class SuffixTree
{
    private readonly NodeCollection _nodes = new();
    private readonly char[] _text;

    public SuffixTree(string line)
    {
        _text = new char[line.Length];

        var position = -1;
        var root = _nodes.Add(new Node(-1, -1));
        var activePoint = new ActivePoint();
        var remainder = 0;

        foreach (var t in line)
        {
            _text[++position] = t;
            var needSuffixLink = -1;
            remainder++;
            while (remainder > 0)
            {
                if (activePoint.Length == 0)
                {
                    activePoint.Edge = position;
                }
            
                if (!_nodes[activePoint.Node].ContainsChild(_text[activePoint.Edge]))
                {
                    var leaf = _nodes.Add(new Node(position));
                    _nodes[activePoint.Node].PutChild(_text[activePoint.Edge], leaf);
                    AddSuffixLink(activePoint.Node, ref needSuffixLink);
                }
                else
                {
                    var next = _nodes[activePoint.Node][_text[activePoint.Edge]];
                    if (WalkDown(next, position, ref activePoint)) continue;
                
                    if (_text[_nodes[next].Start + activePoint.Length] == t)
                    {
                        activePoint.Length++;
                        AddSuffixLink(activePoint.Node, ref needSuffixLink);
                        break;
                    }
                
                    var split = _nodes.Add(new Node(_nodes[next].Start, _nodes[next].Start + activePoint.Length));
                    _nodes[activePoint.Node].PutChild(_text[activePoint.Edge], split);
                    
                    var leaf = _nodes.Add(new Node(position));
                    _nodes[split].PutChild(t, leaf);
                    _nodes[next].Start += activePoint.Length;
                    _nodes[split].PutChild(_text[_nodes[next].Start], next);
                    AddSuffixLink(split, ref needSuffixLink);
                }
            
                remainder--;
            
                if (activePoint.Node == root && activePoint.Length > 0)
                {
                    activePoint.Length--;
                    activePoint.Edge = position - remainder + 1;
                }
                else
                {
                    activePoint.Node = _nodes[activePoint.Node].Link > 0
                        ? _nodes[activePoint.Node].Link
                        : root;   
                }
            }
        }
    }

    public bool Search(string pattern)
    {
        if (pattern.Length > _text.Length) return false;
        
        var root = _nodes[0];
        if (!root.ContainsChild(pattern[0]))
        {
            return false;
        }

        return Traverse(root[pattern[0]], pattern, 0);
    }

    private bool Traverse(int nodeIndex, string pattern, int index)
    {
        var node = _nodes[nodeIndex];
        for (var i = node.Start; i < node.End; i++)
        {
            if (index > pattern.Length - 1 || i > _text.Length - 1)
            {
                return false;
            }
            
            if (_text[i] == pattern[index])
            {
                index++;
            }
            
            if (index == pattern.Length)
            {
                return true;
            }
        }
        
        if (!node.ContainsChild(pattern[index]))
        {
            return false;
        }

        return Traverse(node[pattern[index]], pattern, index);
    }
    
    private void AddSuffixLink(int node, ref int needSuffixLink)
    {
        if (needSuffixLink > 0)
        {
            _nodes[needSuffixLink].Link = node;
        }
        
        needSuffixLink = node;
    }
    
    private bool WalkDown(int next, int position, ref ActivePoint activePoint)
    {
        if (activePoint.Length < _nodes[next].EdgeLength(position)) return false;
        
        activePoint.Edge += _nodes[next].EdgeLength(position);
        activePoint.Length -= _nodes[next].EdgeLength(position);
        activePoint.Node = next;
        
        return true;
    }
}