using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Engines;
using DigitalElectronics.Concepts;

namespace Benchmarking;

[MemoryDiagnoser]
[IterationTime(10)]
[SimpleJob(launchCount: 1, warmupCount: 1, iterationCount: 1)]
public class MiscBenchmarks
{
    private static readonly bool[] ArrayOf32Bools =
    {
        false, true, false, true, false, true, false, true,
        false, true, false, true, false, true, false, true,
        false, true, false, true, false, true, false, true,
        false, true, false, true, false, true, false, true
    };

    private static readonly IEnumerable<Bit> EnumerableOf32Bits = ArrayOf32Bools.Select(b => new Bit(b));

    private static readonly Bit[] ArrayOf32Bits = EnumerableOf32Bits.ToArray();

    private static readonly byte[] ArrayOf4Bytes = { 0xAA, 0xBB, 0xCC, 0xDD };

    //[Benchmark]
    public void FromEnumerableOf32Bools()
    {
        var values = ArrayOf32Bools.AsEnumerable();

        var bitVector = new BitVector32();
        int i = 0;
        foreach (var v in values.Take(32))
            bitVector[1 << i++] = v;
    }

    [Benchmark]
    public IEnumerator<bool> ArrayOf32BoolsAsEnumerableGetEnumerator() => ArrayOf32Bools.AsEnumerable().GetEnumerator();

    [Benchmark]
    public System.Collections.IEnumerator ArrayOf32BoolsGetEnumerator() => ArrayOf32Bools.GetEnumerator();
}
