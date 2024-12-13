using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalElectronics.Concepts;
using DigitalElectronics.Modules.Memory;

namespace Benchmarking;

[MemoryDiagnoser]
public class ROMBenchmarks
{
    private static ROM rom;

    [GlobalSetup]
    public void Setup()
    {
        rom = new(Enumerable.Repeat((byte)0, 256));
        rom.SetInputE(true);
    }

    [Benchmark]
    public void SetInputA_UsingBitArray()
    {
        rom.SetInputA(new BitArray(128, 8));
    }

    [Benchmark]
    public void SetInputA_IndividualBit()
    {
        rom.SetInputA(0, true);
    }

    [Benchmark]
    public BitArray? Output()
    {
        return rom.Output;
    }
}
