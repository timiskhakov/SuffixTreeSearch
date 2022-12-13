using System;
using System.Collections.Generic;

namespace SuffixTreeSearch;

internal class Node
{
    private readonly Dictionary<char, int> _children = new();
    
    internal int Start { get; set; }
    internal int End { get; }
    internal int SuffixLink { get; set; } = -1;

    internal Node(int start, int end)
    {
        Start = start;
        End = end;
    }
    
    internal int EdgeLength(int position)
    {
        return Math.Min(End, position + 1) - Start;
    }
    
    internal int this[char key] => _children[key];

    internal bool Contains(char key)
    {
        return _children.ContainsKey(key);
    }

    internal void Put(char key, int value)
    {
        _children[key] = value;
    }
}