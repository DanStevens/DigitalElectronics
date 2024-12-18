﻿using System;
using DigitalElectronics.Components.Memory;
using DigitalElectronics.Concepts;

namespace Benchmarking;

[MemoryDiagnoser]
public class RegisterBenchmarks
{
    private static Register register = new(8);

    [Benchmark]
    public bool IsReadable()
    {
        return register.IsReadable;
    }

    [Benchmark]
    [GlobalSetup(Target = nameof(Output))]
    public void SetInputE()
    {
        if (!register.IsReadable)
            throw new InvalidOperationException("IsReadable is false");
        register.SetInputE(true);
    }

    [Benchmark] // Does not allocate
    public BitArray ProbeState()
    {
        return register.ProbeState();
    }

    [Benchmark]
    public void Clock()
    {
        register.Clock();
    }

    [Benchmark]
    public BitArray Output()
    {
        return register.Output.Value;
    }

    [Benchmark]
    public void SetInputL()
    {
        register.SetInputL(true);
    }
}