using System;
using DigitalElectronics.Concepts;
using DigitalElectronics.Modules.Counters;

namespace Benchmarking;

[MemoryDiagnoser]
public class BinaryCounterBenchmarks
{
    private BinaryCounter binaryCounter = new(8);

    [Benchmark] // Does not allocate
    public BitArray Output()
    {
        return binaryCounter.Output;
    }

    [Benchmark] // Does not allocate
    public void Clock()
    {
        binaryCounter.Clock();
    }

    [Benchmark] // Does not allocate
    public void Set()
    {
        binaryCounter.Set(new BitArray(0, length: 4));
    }
}
