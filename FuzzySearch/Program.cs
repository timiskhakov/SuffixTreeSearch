using System;

namespace FuzzySearch;

internal static class Program
{
    private static void Main()
    {
        var suffixTree = new SuffixTree("abcxabcd");
        Console.WriteLine(suffixTree.Search("xab"));
    }
}