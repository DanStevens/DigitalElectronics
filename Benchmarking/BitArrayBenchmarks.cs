using System;
using DigitalElectronics.Concepts;

namespace Benchmarking;

[MemoryDiagnoser]
[IterationCount(10)]
public class BitArrayBenchmarks
{
    private static readonly bool[] Bools =
    {
        false, true, false, true, false, true, false, true,
        false, true, false, true, false, true, false, true,
        false, true, false, true, false, true, false, true,
        false, true, false, true, false, true, false, true
    };

    private static readonly Bit[] Bits = Bools.Select(b => new Bit(b)).ToArray();

    private static readonly byte[] Bytes = { 0xAA, 0xBB, 0xCC, 0xDD };

    [Benchmark]
    public BitArray CreateFromArrayOf32Bools() => new BitArray(Bools);

    [Benchmark]
    public BitArray CreateFromEnumerableOf32Bools() => new BitArray(Bools.AsEnumerable());

    [Benchmark]
    public BitArray CreateFromArrayOf4Bytes() => new BitArray(Bytes);

    [Benchmark]
    public BitArray CreateFromInt32() => new BitArray(new int[] { 0xAAAA });

    [Benchmark]
    public BitArray CreateFromArrayOf32Bits() => new BitArray(Bits);
}
