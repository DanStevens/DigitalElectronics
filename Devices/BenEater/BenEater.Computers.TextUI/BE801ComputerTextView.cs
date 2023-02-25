using DigitalElectronics.BenEater.Computers;
using DigitalElectronics.Concepts;
using System.Diagnostics;
using System.Text;

namespace BenEater.Computers.TextUI;

public class BE801ComputerTextView : IDisposable
{
    private readonly BE801Computer _computer;
    bool haltRequested;

    public bool Turbo { get; set; }

    public BE801ComputerTextView(BE801Computer computer)
    {
        _computer = computer ?? throw new ArgumentNullException(nameof(computer));
    }


    public void Run()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.CancelKeyPress += OnCancel;

        int clockCycles = 0;
        var sw = Stopwatch.StartNew();

        while (!haltRequested)
        {
            var outRegister = _computer.ProbeOutRegister();
            Console.Write("{0} {1}   ",
                ToLEDs(outRegister),
                outRegister.ToString(NumberFormat.UnsignedDecimal));
            Console.CursorLeft = 0;
            _computer.Clock();
            clockCycles++;
            
            if (!Turbo)
                Task.Delay(1).Wait();
        }

        Console.WriteLine();
        Console.WriteLine("Did {0} clock cycles in {1}ms which is about {2:F3}Hz",
            clockCycles, sw.ElapsedMilliseconds, clockCycles / (sw.ElapsedMilliseconds / 1000.0));
    }

    void OnCancel(object? sender, ConsoleCancelEventArgs e)
    {
        e.Cancel = true;
        haltRequested = true;
    }

    string ToLEDs(BitArray ba)
    {
        return string.Join(' ', ba.Reverse().Select(b => b ? '●' : '◌'));
    }

    public void Dispose()
    {
        Console.CancelKeyPress -= OnCancel;
    }
}
