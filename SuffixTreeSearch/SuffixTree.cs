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
        var ap = new ActivePoint();
        var remainder = 0;

        for (var i = 0; i < _text.Span.Length; i++)
        {
            var needSuffixLink = -1;
            remainder++;
            while (remainder > 0)
            {
                if (ap.Length == 0) ap.Edge = i;
                if (!_nodes[ap.Node].Contains(_text.Span[ap.Edge]))
                {
                    var leaf = _nodes.Add(new Node(i, _text.Span.Length));
                    _nodes[ap.Node].Put(_text.Span[ap.Edge], leaf);
                    AddSuffixLink(ap.Node, ref needSuffixLink);
                }
                else
                {
                    var next = _nodes[ap.Node][_text.Span[ap.Edge]];
                    if (WalkDown(next, i, ref ap)) continue;

                    if (_text.Span[_nodes[next].Start + ap.Length] == _text.Span[i])
                    {
                        ap.Length++;
                        AddSuffixLink(ap.Node, ref needSuffixLink);
                        break;
                    }

                    var split = _nodes.Add(new Node(_nodes[next].Start, _nodes[next].Start + ap.Length));
                    _nodes[ap.Node].Put(_text.Span[ap.Edge], split);

                    var leaf = _nodes.Add(new Node(i, _text.Span.Length));
                    _nodes[split].Put(_text.Span[i], leaf);
                    _nodes[next].Start += ap.Length;
                    _nodes[split].Put(_text.Span[_nodes[next].Start], next);
                    AddSuffixLink(split, ref needSuffixLink);
                }

                remainder--;

                if (ap.Node == root && ap.Length > 0)
                {
                    ap.Length--;
                    ap.Edge = i - remainder + 1;
                }
                else
                {
                    ap.Node = _nodes[ap.Node].Link > 0
                        ? _nodes[ap.Node].Link
                        : root;
                }
            }
        }
    }

    public int Search(string pattern)
    {
        if (string.IsNullOrEmpty(pattern) || pattern.Length > _text.Span.Length) return -1;
        
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
    
    private bool WalkDown(int next, int position, ref ActivePoint ap)
    {
        if (ap.Length < _nodes[next].EdgeLength(position)) return false;
        
        ap.Edge += _nodes[next].EdgeLength(position);
        ap.Length -= _nodes[next].EdgeLength(position);
        ap.Node = next;
        
        return true;
    }
}