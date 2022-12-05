using Xunit;

namespace FuzzySearch.Tests;

public class SuffixTreeTests
{
    [Theory]
    [InlineData("aa aäa aa", "aäa", 3)]
    [InlineData("abcxabcd", "xab", 3)]
    [InlineData("abcxabcd", "abcxabc", 0)]
    [InlineData("abcxabcd", "bcxabcd", 1)]
    [InlineData("abcxabcd", "cxabc", 2)]
    [InlineData("abcxabcd", "abcxabcd", 0)]
    [InlineData("abcxabcd", "cxabca", -1)]
    [InlineData("abcxabcd", "xyz", -1)]
    [InlineData("abcxabcd", "abcxabcdb", -1)]
    public void CtorTests(string text, string pattern, int expected)
    {
        Assert.Equal(expected, new SuffixTree(text).Search(pattern));
    }
}