using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalElectronics.Modules.Counters;

namespace Benchmarking;

[MemoryDiagnoser]
public class ProgramCounterBenchmarks
{
    private static ProgramCounter programCounter = new(4);

    [Benchmark]
    public void Clock()
    {
        programCounter.Clock();
    }
}
