using System;
using DigitalElectronics.Components.Memory;

namespace Benchmarking;

[MemoryDiagnoser]
public class RegisterBitBenchmarks
{
    private static RegisterBit registerBit = new();

    [Benchmark]
    public void SetInputE()
    {
        registerBit.SetInputE(true);
    }

    [Benchmark]
    public void SetInputL()
    {
        registerBit.SetInputL(true);
    }
}
