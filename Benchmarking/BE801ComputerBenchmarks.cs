using DigitalElectronics.BenEater.Computers;
using static DigitalElectronics.BenEater.Computers.BE801Computer;

namespace Benchmarking;

[MemoryDiagnoser]
public class BE801ComputerBenchmarks
{
    private static readonly byte[] ZeroProgram = Enumerable.Repeat((byte)0, 16).ToArray();
    private static BE801Computer _subject;

    [GlobalSetup]
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
    [Arguments(10000)]
    public void Clock(int clockCycles)
    {
        for (int i = 0; i < clockCycles; i++)
        {
            _subject.Clock();
        }
    }

    //[Benchmark]
    [Arguments(1)]
    [Arguments(10)]
    [Arguments(100)]
    [Arguments(1000)]
    [Arguments(10000)]
    public void PerformControlLogic(int times)
    {
        for (int i = 0; i < times; i++)
        {
            _subject.PerformControlLogic();
        }
    }

    //[Benchmark]
    [Arguments(ControlSignals._)]
    [Arguments(ControlSignals.Unused)]
    [Arguments(ControlSignals.J)]
    [Arguments(ControlSignals.CO)]
    [Arguments(ControlSignals.CE)]
    [Arguments(ControlSignals.OI)]
    [Arguments(ControlSignals.BI)]
    [Arguments(ControlSignals.SU)]
    [Arguments(ControlSignals.EO)]
    [Arguments(ControlSignals.AO)]
    [Arguments(ControlSignals.AI)]
    [Arguments(ControlSignals.II)]
    [Arguments(ControlSignals.IO)]
    [Arguments(ControlSignals.RO)]
    [Arguments(ControlSignals.RI)]
    [Arguments(ControlSignals.MI)]
    [Arguments(ControlSignals.Halt)]
    public void SetControlSignal(ControlSignals cs)
    {
        _subject.SetControlSignal(cs);
    }
}
