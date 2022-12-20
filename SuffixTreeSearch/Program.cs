using System;
using System.IO;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace SuffixTreeSearch;

[MemoryDiagnoser]
public class Program
{
    private const string Pattern = "gZlXHlxthQ7tHFYlFSme640DPQp";
    private string _text = null!;

    [Params(1_000, 10_000, 100_000, 1_000_000, 10_000_000)]
    public int N { get; set; }

    [GlobalSetup]
    public async Task Setup()
    {
        var file = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "64k.txt");
        _text = await File.ReadAllTextAsync(file);
    }
    
    [Benchmark]
    public void IndexOf()
    {
        for (var i = 0; i <= N; i++)
        {
            var _ = _text.IndexOf(Pattern, StringComparison.Ordinal);   
        }
    }

    [Benchmark]
    public void SuffixTree()
    {
        var suffixTree = new SuffixTree(_text);
        for (var i = 0; i <= N; i++)
        {
            var _ = suffixTree.Search(Pattern);
        }
    }
    
    private static void Main()
    {
        BenchmarkRunner.Run<Program>();
    }
}