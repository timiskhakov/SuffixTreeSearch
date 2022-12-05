using System;
using System.Collections.Generic;
using System.Linq;

namespace FuzzySearch;

internal class Node
{
    internal int Start { get; set; }
    internal int End { get; }
    internal int Link { get; set; }
    
    private readonly Dictionary<char, int> _next = new();

    internal Node(int start, int end = int.MaxValue)
    {
        Start = start;
        End = end;
    }

    // internal int CountChildren()
    // {
    //     return _next.Count;
    // }
    //
    // internal int GetValueByIndex(int index)
    // {
    //     return _next.ElementAt(index).Value;
    // }
    
    internal int GetValueByKey(char key)
    {
        return _next[key];
    }

    internal bool ContainsKey(char key)
    {
        return _next.ContainsKey(key);
    }

    internal void AddOrUpdate(char key, int value)
    {
        if (_next.ContainsKey(key))
        {
            _next[key] = value;
        }
        else
        {
            _next.Add(key, value);   
        }
    }

    internal int EdgeLength(int position)
    {
        return Math.Min(End, position + 1) - Start;
    }
}