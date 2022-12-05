using System;

namespace FuzzySearch;

internal static class Program
{
    private static void Main()
    {
        var suffixTree = new SuffixTree("aaaäaaa");
        Console.WriteLine(suffixTree.Search("aäa"));
    }
}