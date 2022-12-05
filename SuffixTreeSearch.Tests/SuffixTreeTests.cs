using Xunit;

namespace SuffixTreeSearch.Tests;

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
    public void Searches(string text, string pattern, int expected)
    {
        Assert.Equal(expected, new SuffixTree(text).Search(pattern));
    }

    [Fact]
    public void SearchMultiplyQueries()
    {
        const string text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent eleifend lectus ut euismod bibendum. Mauris orci massa, ornare a fermentum in, mollis sed lectus. Morbi ornare tincidunt neque, sed porta magna eleifend non. Suspendisse vulputate vitae augue eget pharetra. Donec rhoncus rhoncus ligula et fringilla. Proin vitae dignissim est. Etiam sit amet efficitur risus, nec aliquet sem. Suspendisse potenti. Donec aliquet auctor neque, quis malesuada tellus semper quis. Mauris non faucibus libero, ac iaculis nisl. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed tempus dolor et leo accumsan accumsan maximus eget velit.";
        
        var suffixTree = new SuffixTree(text);

        Assert.Equal(0, suffixTree.Search("Lorem"));
        Assert.Equal(6, suffixTree.Search("ipsum"));
        Assert.Equal(633, suffixTree.Search("velit"));
    }
}