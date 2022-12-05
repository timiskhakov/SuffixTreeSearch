using System;

namespace SuffixTreeSearch;

internal static class Program
{
    private static void Main()
    {
        var suffixTree = new SuffixTree("abcxabcd");
        Console.WriteLine(suffixTree.Search("xab"));
    }
}