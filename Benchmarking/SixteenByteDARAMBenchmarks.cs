using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalElectronics.Modules.Memory;

namespace Benchmarking;

[MemoryDiagnoser]
public class SixteenByteDARAMBenchmarks
{
    private SixteenByteDARAM ram = new();

    [Benchmark]
    public void Clock()
    {
        ram.Clock();
    }
}
