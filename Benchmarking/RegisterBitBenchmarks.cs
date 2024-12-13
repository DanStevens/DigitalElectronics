using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
}
