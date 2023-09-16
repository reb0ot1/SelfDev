using BenchmarkDotNet.Attributes;
using System.Runtime.InteropServices;
using SelfDevelopmentProj.LINQ;

namespace SelfDevelopmentProj.LINQ;

//[SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.Net60)]
[SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.Net70)]
[MemoryDiagnoser(false)]
public class Benchmarks
{
    [Params(100)]
    public int Size { get; set; }

    private IEnumerable<int> _items;
    private List<int> _items2;

    [GlobalSetup]
    public void Setup()
    {
        _items = Enumerable.Range(1, Size).ToArray();
        _items2 = Enumerable.Range(1, Size).ToList();
    }

    //[Benchmark]
    //public int GetCountByFilteredValue()
    //    => _items.Where(x => x > 50).Count();

    [Benchmark]
    public bool GetCountByFilteredValue2()
        => _items.Any();

    [Benchmark]
    public bool GetCountByFilteredValue3()
        => _items.Count() > 0;

    //[Benchmark]
    //public int GetMax()
    //    => _items.Max();

    //[Benchmark]
    //public int GetMin()
    //    => _items.Min();

    //[Benchmark]
    //public double GetAverage()
    //    => _items.Average();

    //[Benchmark]
    //public int Sum()
    //    => _items.Sum();

    //[Benchmark]
    //public bool IfAny()
    //    => _items.Any();

    //[Benchmark]
    //public bool IfAnyOwn()
    //    => _items.Count() > 0;

    public void For_Span()
    {
        var asSpan = CollectionsMarshal.AsSpan(_items2);
        for (int i = 0; i < asSpan.Length; i++)
        {
            var item = asSpan[i];
        }
    }

    public void ExtendedForLoop()
    {
        foreach (var i in 1..10)
        {
            Console.Write(i);
        }
    }
}
