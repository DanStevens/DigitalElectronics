using DigitalElectronics.BenEater.Computers;
using DigitalElectronics.Concepts;
using System.Diagnostics;
using System.Text;

namespace BenEater.Computers.TextUI;

/// <summary>
/// Writes a text-based UI for the BE801 Computer to the console
/// </summary>
public class BE801ComputerTextView : IDisposable
{
    private readonly BE801Computer _computer;
    bool haltRequested;

    private const string InterfaceHeader = """
        ╔═════════════════════════════════╗
        ║         BE801 Computer          ║
        ║ Program:                        ║
        """;

    private const string InterfaceMain = """
        ╠═════════════════════════════════╣
        ║         Output Register         ║
        ║         ● ● ● ● ● ● ● ●         ║
        ║         0xFF  -128  255         ║
        ╚═════════════════════════════════╝
        """;

    /// <summary>
    /// By default, a 1ms wait it placed between each call to
    /// <see cref="Clock"/>. When this is set to <c>true</c>, there is no wait
    /// and so the computer runs as fast as possible.
    /// </summary>
    public bool Turbo { get; set; }

    /// <summary>
    /// The name of the program to display in the header
    /// </summary>
    public string ProgramName { get; set; }

    public BE801ComputerTextView(BE801Computer computer)
    {
        _computer = computer ?? throw new ArgumentNullException(nameof(computer));
    }

    /// <summary>
    /// Runs the computer (by continuously calling <see cref="BE801Computer.Clock"/>
    /// method in a loop), until halted by the user
    /// </summary>
    /// <remarks>Press Ctrl+C to halt the computer and terminate the app</remarks>
    public void Run()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.CursorVisible = false;
        Console.Clear();
        OutputHeaderUI();

        Console.CancelKeyPress += OnCancel;

        int clockCycles = 0;
        var sw = Stopwatch.StartNew();

        while (!haltRequested)
        {
            OutputComputerUI();
            _computer.Clock();
            clockCycles++;

            if (!Turbo)
                Task.Delay(1).Wait();
        }

        Console.SetCursorPosition(0, 8);
        Console.WriteLine("Did {0} clock cycles in {1}ms which is about {2:F3}Hz",
            clockCycles, sw.ElapsedMilliseconds, clockCycles / (sw.ElapsedMilliseconds / 1000.0));
    }

    void OutputHeaderUI()
    {
        Console.SetCursorPosition(0, 0);
        Console.WriteLine(InterfaceHeader);
        
        // Write program name to UI
        WriteAtPosition(ProgramName, 11, 2, 17);
    }

    void OutputComputerUI()
    {
        Console.SetCursorPosition(0, 3);
        Console.WriteLine(InterfaceMain);

        // Write output register to UI
        var outRegister = _computer.ProbeOutRegister();

        // As binary 'LEDs'
        WriteAtPosition(ToLEDs(outRegister), 10, 5, 15); 
        
        // As unsigned Hex
        WriteAtPosition("0x" + outRegister.ToString(NumberFormat.UnsignedHexadecimal),
            10, 6, 4);

        // As signed decimal
        WriteAtPosition(outRegister.ToString(NumberFormat.SignedDecimal).PadLeft(4),
            16, 6, 4);

        // As unsigned decimal
        WriteAtPosition(outRegister.ToString(NumberFormat.UnsignedDecimal).PadLeft(3),
            22, 6, 3);
    }

    void WriteAtPosition(string text, int left, int top, int length)
    {
        Console.SetCursorPosition(left, top);
        var adjustedText = text.Length <= length
            ? text.PadRight(length) : Truncate(text, length); 
        Console.Write(adjustedText);
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

    string Truncate(string s, int length)
    {
        if (s.Length < length)
            return s;

        return s.Substring(0, length - 1) + "…";
    }

    public void Dispose()
    {
        Console.CancelKeyPress -= OnCancel;
        Console.CursorVisible = true;
    }
}
