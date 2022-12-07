using System;

namespace SuffixTreeSearch;

public class SuffixTree
{
    private readonly NodeCollection _nodes = new();
    private readonly ReadOnlyMemory<char> _text;

    public SuffixTree(string line)
    {
        _text = line.AsMemory();

        var root = _nodes.Add(new Node(-1, -1));
        var point = new ActivePoint();
        var remainder = 0;

        for (var i = 0; i < _text.Span.Length; i++)
        {
            var needSuffixLink = -1;
            remainder++;
            while (remainder > 0)
            {
                if (point.Length == 0) point.Edge = i;
                if (!_nodes[point.Node].Contains(_text.Span[point.Edge]))
                {
                    var leaf = _nodes.Add(new Node(i, _text.Span.Length));
                    _nodes[point.Node].Put(_text.Span[point.Edge], leaf);
                    AddSuffixLink(point.Node, ref needSuffixLink);
                }
                else
                {
                    var next = _nodes[point.Node][_text.Span[point.Edge]];
                    if (WalkDown(next, i, ref point)) continue;

                    if (_text.Span[_nodes[next].Start + point.Length] == _text.Span[i])
                    {
                        point.Length++;
                        AddSuffixLink(point.Node, ref needSuffixLink);
                        break;
                    }

                    var split = _nodes.Add(new Node(_nodes[next].Start, _nodes[next].Start + point.Length));
                    _nodes[point.Node].Put(_text.Span[point.Edge], split);

                    var leaf = _nodes.Add(new Node(i, _text.Span.Length));
                    _nodes[split].Put(_text.Span[i], leaf);
                    _nodes[next].Start += point.Length;
                    _nodes[split].Put(_text.Span[_nodes[next].Start], next);
                    AddSuffixLink(split, ref needSuffixLink);
                }

                remainder--;

                if (point.Node == root && point.Length > 0)
                {
                    point.Length--;
                    point.Edge = i - remainder + 1;
                }
                else
                {
                    point.Node = _nodes[point.Node].Link > 0
                        ? _nodes[point.Node].Link
                        : root;
                }
            }
        }
    }

    public int Search(string pattern)
    {
        if (pattern.Length > _text.Span.Length) return -1;
        
        var node = _nodes[0];
        var c = pattern[0];
        var patternIdx = 0;
        
        while (node.Contains(c))
        {
            var nodeIdx = node[c];
            node = _nodes[nodeIdx];
            for (var i = node.Start; i < node.End; i++)
            {
                if (_text.Span[i] == pattern[patternIdx]) patternIdx++;
                if (patternIdx == pattern.Length) return i - patternIdx + 1;
            }

            c = pattern[patternIdx];
        }

        return -1;
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