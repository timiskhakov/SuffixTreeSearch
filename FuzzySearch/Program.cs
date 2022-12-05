namespace FuzzySearch;

internal static class Program
{
    private static void Main()
    {
        var suffixTree = new SuffixTree("abcxabcd");
        var q = suffixTree.Search(      "bcxab");
    }
}