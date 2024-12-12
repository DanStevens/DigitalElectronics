using System;
using System.Collections.Specialized;
using BenchmarkDotNet.Engines;
using DigitalElectronics.Concepts;
using DigitalElectronics.Utilities;
using DigitalElectronics.Components.LogicGates;
using Buffer = DigitalElectronics.Components.LogicGates.Buffer;
using Microsoft.CodeAnalysis.Operations;
using System.Collections.ObjectModel;

namespace Benchmarking;

[MemoryDiagnoser]
//[IterationTime(10)]
//[SimpleJob(launchCount: 1, warmupCount: 1, iterationCount: 1)]
public class BitArrayInstantiationBenchmarks
{
    private static readonly bool[] ArrayOf32Bools =
    {
        false, true, false, true, false, true, false, true,
        false, true, false, true, false, true, false, true,
        false, true, false, true, false, true, false, true,
        false, true, false, true, false, true, false, true
    };

    private static readonly List<bool> ListOf32Bools = new List<bool>(ArrayOf32Bools);
    
    private static readonly ObservableCollection<bool> ObservableCollectionOf32Bools = new ObservableCollection<bool>(ArrayOf32Bools.AsEnumerable());

    private static readonly IEnumerable<Bit> EnumerableOf32Bits = ArrayOf32Bools.Select(b => new Bit(b));

    private static readonly Bit[] ArrayOf32Bits = EnumerableOf32Bits.ToArray();

    private static readonly IList<Bit> ListOf32Bits = new List<Bit>(EnumerableOf32Bits);

    private static readonly ObservableCollection<Bit> ObservableCollectionOf32Bits = new ObservableCollection<Bit>(EnumerableOf32Bits);

    private static readonly byte[] ArrayOf4Bytes = { 0xAA, 0xBB, 0xCC, 0xDD };

    private static readonly Buffer[] ArrayOf32Buffers = Enumerable.Range(0, 32).Select(_ => new Buffer()).ToArray();

    [Benchmark]
    public BitArray Default() => new BitArray();

    [Benchmark]
    public BitArray FromArrayOf32Bools() => new BitArray(ArrayOf32Bools);

    [Benchmark]
    public BitArray FromSpanOf32Bools() => new BitArray(ArrayOf32Bools.AsSpan());

    [Benchmark]
    public BitArray FromEnumerableOf32Bools() => new BitArray(ArrayOf32Bools.AsEnumerable());

    [Benchmark]
    public BitArray FromEnumerableOf32BoolsAndLength() => new BitArray(ArrayOf32Bools.AsEnumerable(), 32);

    [Benchmark]
    public BitArray FromCollectionOf32Bools() => new BitArray((ICollection<bool>)ArrayOf32Bools);

    [Benchmark]
    public BitArray FromListOf32Bools() => BitArray.FromList(ListOf32Bools);

    [Benchmark]
    public BitArray FromObservableCollectionOf32Bools() => BitArray.FromList(ObservableCollectionOf32Bools);

    [Benchmark]
    public BitArray FromArrayOf4Bytes() => BitArray.FromBytes(ArrayOf4Bytes);

    [Benchmark]
    public BitArray FromArrayOf32Bits() => ArrayOf32Bits.ToBitArray();

    [Benchmark]
    public BitArray FromEnumerableOf32Bits() => ArrayOf32Bits.AsEnumerable().ToBitArray();

    [Benchmark]
    public BitArray FromEnumerableOf32BitsAndLength() => ArrayOf32Bits.AsEnumerable().ToBitArray(32);

    [Benchmark]
    public BitArray FromEnumerableOf32Bits_UsingSelect() => new BitArray(ArrayOf32Bits.Select(b => b.Value));

    [Benchmark]
    public BitArray FromCollectionOf32Bits() => ((ICollection<Bit>)ArrayOf32Bits).ToBitArray();

    [Benchmark]
    public BitArray FromListOf32Bits() => ListOf32Bits.ToBitArray();

    [Benchmark]
    public BitArray FromObservableCollectionOf32Bits() => ObservableCollectionOf32Bits.ToBitArray();

    [Benchmark]
    public BitArray FromListOf32Bits_UsingSelect() => new BitArray(ListOf32Bits.Select(b => b.Value));

    [Benchmark]
    public BitArray FromListOf32Bits_UsingSelectAndLength() => new BitArray(ListOf32Bits.Select(b => b.Value), ListOf32Bits.Count);

    //[Benchmark]
    //public BitArray FromArrayOf32Buffers() => ArrayOf32Buffers.ToBitArray();

    [Benchmark]
    public BitArray FromArrayOf32Buffers_UsingSelect() => new BitArray(ArrayOf32Buffers.Select(b => b.OutputQ));

    [Benchmark]
    public BitArray FromArrayOf32Buffers_UsingSelectAndLength() => new BitArray(ArrayOf32Buffers.Select(b => b.OutputQ), ArrayOf32Buffers.Length);
}
