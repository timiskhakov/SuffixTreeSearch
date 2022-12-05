namespace FuzzySearch;

public class SuffixTree
{
    private readonly Node[] _nodes;
    private readonly char[] _text;

    private readonly int _root;
    private int _position = -1;
    private int _currentNode = -1;
    private int _needSuffixLink;
    private int _remainder;

    public SuffixTree(string line)
    {
        _nodes = new Node[2 * line.Length + 2];
        _text = new char[line.Length];
        _root = NewNode(-1, -1);

        var activePoint = new ActivePoint();
        
        for (var i = 0; i < line.Length; i++)
        {
            _text[++_position] = line[i];
            _needSuffixLink = -1;
            _remainder++;
            while (_remainder > 0)
            {
                if (activePoint.Length == 0)
                {
                    activePoint.Edge = _position;
                }
            
                if (!_nodes[activePoint.Node].ContainsKey(_text[activePoint.Edge]))
                {
                    var leaf = NewNode(_position);
                    _nodes[activePoint.Node].AddOrUpdate(_text[activePoint.Edge], leaf);
                    AddSuffixLink(activePoint.Node);
                }
                else
                {
                    var next = _nodes[activePoint.Node].GetValueByKey(_text[activePoint.Edge]);
                    if (WalkDown(next, ref activePoint)) continue;
                
                    if (_text[_nodes[next].Start + activePoint.Length] == line[i])
                    {
                        activePoint.Length++;
                        AddSuffixLink(activePoint.Node);
                        break;
                    }
                
                    var split = NewNode(_nodes[next].Start, _nodes[next].Start + activePoint.Length);
                    _nodes[activePoint.Node].AddOrUpdate(_text[activePoint.Edge], split);
                
                    var leaf = NewNode(_position);
                    _nodes[split].AddOrUpdate(line[i], leaf);
                    _nodes[next].Start += activePoint.Length;
                    _nodes[split].AddOrUpdate(_text[_nodes[next].Start], next);
                    AddSuffixLink(split);
                }
            
                _remainder--;
            
                if (activePoint.Node == _root && activePoint.Length > 0)
                {
                    activePoint.Length--;
                    activePoint.Edge = _position - _remainder + 1;
                }
                else
                {
                    activePoint.Node = _nodes[activePoint.Node].Link > 0
                        ? _nodes[activePoint.Node].Link
                        : _root;   
                }
            }
        }
    }

    public bool Search(string pattern)
    {
        if (pattern.Length > _text.Length) return false;
        
        var root = _nodes[0];
        if (!root.ContainsKey(pattern[0]))
        {
            return false;
        }

        return Traverse(root.GetValueByKey(pattern[0]), pattern, 0);
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
        
        if (!node.ContainsKey(pattern[index]))
        {
            return false;
        }

        return Traverse(node.GetValueByKey(pattern[index]), pattern, index);
    }

    private int NewNode(int start, int end = int.MaxValue)
    {
        _nodes[++_currentNode] = new Node(start, end);
        return _currentNode;
    }
    
    private void AddSuffixLink(int node)
    {
        if (_needSuffixLink > 0)
        {
            _nodes[_needSuffixLink].Link = node;
        }
        
        _needSuffixLink = node;
    }
    
    private bool WalkDown(int next, ref ActivePoint activePoint)
    {
        if (activePoint.Length < _nodes[next].EdgeLength(_position)) return false;
        
        activePoint.Edge += _nodes[next].EdgeLength(_position);
        activePoint.Length -= _nodes[next].EdgeLength(_position);
        activePoint.Node = next;
        
        return true;
    }
}