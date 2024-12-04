using DigitalElectronics.BenEater.Computers;

namespace Benchmarking;

[MemoryDiagnoser]
public class BE801ComputerBenchmarks
{
    private static readonly byte[] ZeroProgram = Enumerable.Repeat((byte)0, 16).ToArray();
    private BE801Computer _subject;

    [IterationSetup]
    public void Setup()
    {
        _subject = new BE801Computer();
        _subject.LoadRAM(ZeroProgram);
    }

    [Benchmark]
    [Arguments(1)]
    [Arguments(10)]
    [Arguments(100)]
    [Arguments(1000)]
    public void Clock(int clockCycles)
    {
        for (int i = 0; i < clockCycles; i++)
        {
            _subject.Clock();
        }
    }
}
