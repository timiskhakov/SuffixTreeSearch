using Xunit;

namespace FuzzySearch.Tests;

public class SuffixTreeTests
{
    [Theory]
    [InlineData("aa aäa aa", "aäa", true)]
    [InlineData("abcxabcd", "xab", true)]
    [InlineData("abcxabcd", "abcxabc", true)]
    [InlineData("abcxabcd", "bcxabcd", true)]
    [InlineData("abcxabcd", "cxabc", true)]
    [InlineData("abcxabcd", "abcxabcd", true)]
    [InlineData("abcxabcd", "cxabca", false)]
    [InlineData("abcxabcd", "xyz", false)]
    [InlineData("abcxabcd", "abcxabcdb", false)]
    public void CtorTests(string text, string pattern, bool expected)
    {
        Assert.Equal(expected, new SuffixTree(text).Search(pattern));
    }
}