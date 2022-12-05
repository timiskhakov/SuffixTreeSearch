using System;

namespace FuzzySearch;

public class SuffixTree
{
    private readonly NodeCollection _nodes = new();
    private readonly ReadOnlyMemory<char> _text;

    public SuffixTree(string line)
    {
        _text = line.AsMemory();

        var root = _nodes.Add(new Node(-1, -1));
        var activePoint = new ActivePoint();
        var remainder = 0;

        for (var i = 0; i < _text.Span.Length; i++)
        {
            var needSuffixLink = -1;
            remainder++;
            while (remainder > 0)
            {
                if (activePoint.Length == 0)
                {
                    activePoint.Edge = i;
                }

                if (!_nodes[activePoint.Node].ContainsChild(_text.Span[activePoint.Edge]))
                {
                    var leaf = _nodes.Add(new Node(i, _text.Span.Length));
                    _nodes[activePoint.Node].PutChild(_text.Span[activePoint.Edge], leaf);
                    AddSuffixLink(activePoint.Node, ref needSuffixLink);
                }
                else
                {
                    var next = _nodes[activePoint.Node][_text.Span[activePoint.Edge]];
                    if (WalkDown(next, i, ref activePoint)) continue;

                    if (_text.Span[_nodes[next].Start + activePoint.Length] == _text.Span[i])
                    {
                        activePoint.Length++;
                        AddSuffixLink(activePoint.Node, ref needSuffixLink);
                        break;
                    }

                    var split = _nodes.Add(new Node(_nodes[next].Start, _nodes[next].Start + activePoint.Length));
                    _nodes[activePoint.Node].PutChild(_text.Span[activePoint.Edge], split);

                    var leaf = _nodes.Add(new Node(i, _text.Span.Length));
                    _nodes[split].PutChild(_text.Span[i], leaf);
                    _nodes[next].Start += activePoint.Length;
                    _nodes[split].PutChild(_text.Span[_nodes[next].Start], next);
                    AddSuffixLink(split, ref needSuffixLink);
                }

                remainder--;

                if (activePoint.Node == root && activePoint.Length > 0)
                {
                    activePoint.Length--;
                    activePoint.Edge = i - remainder + 1;
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
        if (pattern.Length > _text.Span.Length) return false;
        
        var root = _nodes[0];
        if (!root.ContainsChild(pattern[0]))
        {
            return false;
        }

        var nodeIndex = root[pattern[0]];
        var index = 0;
        
        while (true)
        {
            var node = _nodes[nodeIndex];
            for (var i = node.Start; i < node.End; i++)
            {
                if (_text.Span[i] == pattern[index]) index++;
                if (index == pattern.Length) return true;
            }

            if (!node.ContainsChild(pattern[index])) return false;

            nodeIndex = node[pattern[index]];
        }
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