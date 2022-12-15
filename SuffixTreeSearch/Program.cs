using System;

namespace SuffixTreeSearch;

internal static class Program
{
    private static void Main()
    {
        var suffixTree = new SuffixTree("velvetveil");
        Console.WriteLine(suffixTree.Search("etv"));
    }
}