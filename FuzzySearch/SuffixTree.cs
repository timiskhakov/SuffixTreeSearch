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

    private int _activeNode;
    private int _activeLength;
    private int _activeEdge;
    
    public SuffixTree(string line)
    {
        _nodes = new Node[2 * line.Length + 2];
        _text = new char[line.Length];
        _root = NewNode(-1, -1);
        _activeNode = _root;
        
        for (var i = 0; i < line.Length; i++)
        {
            Add(line[i]);
        }
    }

    private void Add(char ch)
    {
        _text[++_position] = ch;
        _needSuffixLink = -1;
        _remainder++;
        while (_remainder > 0)
        {
            if (_activeLength == 0)
            {
                _activeEdge = _position;
            }
            
            if (!_nodes[_activeNode].ContainsKey(_text[_activeEdge]))
            {
                var leaf = NewNode(_position);
                _nodes[_activeEdge].AddOrUpdate(_text[_activeEdge], leaf);
                AddSuffixLink(_activeNode);
            }
            else
            {
                var next = _nodes[_activeNode].GetValue(_text[_activeEdge]);
                if (WalkDown(next)) continue;
                
                if (_text[_nodes[next].Start + _activeLength] == ch)
                {
                    _activeLength++;
                    AddSuffixLink(_activeNode);
                    break;
                }
                
                var split = NewNode(_nodes[next].Start, _nodes[next].Start + _activeLength);
                _nodes[_activeNode].AddOrUpdate(_text[_activeEdge], split);
                
                var leaf = NewNode(_position);
                _nodes[split].AddOrUpdate(ch, leaf);
                _nodes[next].Start +=_activeLength;
                _nodes[split].AddOrUpdate(_text[_nodes[next].Start], next);
                AddSuffixLink(split);
            }
            
            _remainder--;
            
            if (_activeNode == _root && _activeLength > 0)
            {
                _activeLength--;
                _activeEdge = _position - _remainder + 1;
            }
            else
            {
                _activeNode = _nodes[_activeNode].Link > 0
                    ? _nodes[_activeNode].Link
                    : _root;   
            }
        }
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
    
    private bool WalkDown(int next)
    {
        if (_activeLength < _nodes[next].EdgeLength(_position)) return false;
        
        _activeEdge += _nodes[next].EdgeLength(_position);
        _activeLength -= _nodes[next].EdgeLength(_position);
        _activeNode = next;
        
        return true;
    }
}