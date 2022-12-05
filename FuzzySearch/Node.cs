using System;
using System.Collections.Generic;

namespace FuzzySearch;

internal class Node
{
    internal int Start { get; set; }
    internal int End { get; }
    internal int Link { get; set; }
    
    private readonly Dictionary<char, int> _children = new();

    internal Node(int start, int end)
    {
        Start = start;
        End = end;
    }
    
    internal int this[char key] => _children[key];
    
    internal int EdgeLength(int position)
    {
        return Math.Min(End, position + 1) - Start;
    }

    internal bool Contains(char key)
    {
        return _children.ContainsKey(key);
    }

    internal void Put(char key, int value)
    {
        _children[key] = value;
    }
}