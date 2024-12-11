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
    private bool haltRequested;
    private bool clockLedState;

    public static string Usage => $"""
        Usage: {AppDomain.CurrentDomain.FriendlyName} <program> [/turbo | /wait:50]"
        
        <program>     Path to a binary file containing a program to run in machine code
        /turbo        Runs the computer as fast as possible
        /wait:<ms>    Wait time between clock cycles in milliseconds (default: 50)
        """;

    private const string InterfaceHeader = """
        ╔═════════════════════════════════════════════════════════════════════════════╗
        ║                               BE801 Computer                                ║
        ║ Program:                                                                    ║
        ╠═════════════════════════════════╦═════════╦═════════════════════════════════╣
        """;

    private const string InterfaceMain = """
        ║             Clock ◌             ║   Bus   ║         Program Counter         ║
        ║                                 ║         ║             ● ● ● ●             ║
        ║      Clock speed:  10000Hz      ║         ║             0xF  15             ║
        ╠═════════════════════════════════╣         ╠═════════════════════════════════╣
        ║     Memory Address Register     ║         ║                                 ║
        ║            ● ● ● ●              ║         ║            A Register           ║
        ║            0xF  15              ║         ║         ● ● ● ● ● ● ● ●         ║
        ╟─────────────────────────────────╢         ║         0xFF  -128  255         ║
        ║           16-Byte RAM           ║         ╟─────────────────────────────────╢
        ║ 0:●●●●●●●● 255   8:●●●●●●●● 255 ║         ║               ALU               ║
        ║ 1:●●●●●●●● 255   9:●●●●●●●● 255 ║         ║         ◌ ◌ ◌ ◌ ◌ ◌ ◌ ◌         ║
        ║ 2:●●●●●●●● 255  10:●●●●●●●● 255 ║         ║         0x00     0    0         ║
        ║ 3:●●●●●●●● 255  11:●●●●●●●● 255 ║         ╟─────────────────────────────────╢
        ║ 4:●●●●●●●● 255  12:●●●●●●●● 255 ║         ║            B Register           ║
        ║ 5:●●●●●●●● 255  13:●●●●●●●● 255 ║         ║         ● ● ● ● ● ● ● ●         ║
        ║ 6:●●●●●●●● 255  14:●●●●●●●● 255 ║         ║         0xFF  -128  255         ║
        ║ 7:●●●●●●●● 255  15:●●●●●●●● 255 ║         ║                                 ║
        ╠═════════════════════════════════╣         ╠═════════════════════════════════╣
        ║      Instruction Register       ║         ║         Output Register         ║
        ║       ● ● ● ●     ● ● ● ●       ║         ║         ● ● ● ● ● ● ● ●         ║
        ║       HLT  15     0xF  15       ║         ║         0xFF  -128  255         ║
        ╚═════════════════════════════════╩═════════╩═════════════════════════════════╝
        """;

    /// <summary>
    /// When set to <c>true</c>, disables waiting between clock cycles, meaning
    /// the BE801 will run at the fastest speed possible, otherwise there is a
    /// delay between clock cycles as defined by <see cref="WaitTimeBetweenClockCycles"/>
    /// </summary>
    public bool Turbo { get; set; }

    /// <summary>
    /// The name of the program to display in the header
    /// </summary>
    public string ProgramName { get; set; } = string.Empty;

    /// <summary>
    /// Time to wait between clock cycles in milliseconds
    /// </summary>
    public int WaitTimeBetweenClockCycles { get; set; } = 50;


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

        while (!haltRequested && !_computer.HaltFlag)
        {
            OutputComputerUI();
            _computer.Clock();
            clockCycles++;

            if (!Turbo)
                Task.Delay(WaitTimeBetweenClockCycles).Wait();
        }

        Console.SetCursorPosition(0, 27);
        if (_computer.HaltFlag)
            Console.WriteLine("Computer was halted by HLT instruction");
        Console.WriteLine("Did {0} clock cycles in {1}ms which is about {2:F3}Hz",
            clockCycles, sw.ElapsedMilliseconds, clockCycles / (sw.ElapsedMilliseconds / 1000.0));
    }

    void OutputHeaderUI()
    {
        Console.SetCursorPosition(0, 0);
        Console.WriteLine(InterfaceHeader);
        
        // Write program name to UI
        WriteAtPosition(ProgramName, 11, 2, 66);
    }

    void OutputComputerUI()
    {
        Console.SetCursorPosition(0, 4);
        Console.WriteLine(InterfaceMain);

        // Alternate the Clock LED
        clockLedState = !clockLedState;
        Console.SetCursorPosition(20, 4);
        Console.Write(ToLED(clockLedState));

        OutputHalfRegisterValue(_computer.ProbePC(), 58, 5);
        OutputHalfRegisterValue(_computer.ProbeMAR(), 13, 9);
        OutputFullRegisterValue(_computer.ProbeARegister(), 54, 10);
        OutputFullRegisterValue(_computer.ProbeALU(), 54, 14);
        OutputFullRegisterValue(_computer.ProbeBRegister(), 54, 18);
        OutputInstructionRegister();
        OutputFullRegisterValue(_computer.ProbeOutRegister(), 54, 23);
    }

    private void OutputInstructionRegister()
    {
        var instrRegister = _computer.ProbeInstrRegister();
        var opcode = new BitArray(instrRegister.AsEnumerable().TakeLast(4));
        var operand = new BitArray(instrRegister.AsEnumerable().Take(4));

        OutputOpcode(opcode, 8, 23);
        OutputOperand(operand, 20, 23);

        void OutputOpcode(BitArray opcode, int left, int top)
        {
            // As binary 'LEDs'
            WriteAtPosition(ToLEDs(opcode), left, top, 7);

            // As mnemonics
            WriteAtPosition(ToMnemonic(opcode.ToByte()), left, top + 1, 3);

            // As unsigned decimal
            WriteAtPosition(opcode.ToString(NumberFormat.UnsignedDecimal).PadLeft(2),
                left + 5, top + 1, 2);
        }

        void OutputOperand(BitArray operand, int left, int top)
        {
            OutputHalfRegisterValue(operand, left, top);
        }
    }



    /// <summary>
    /// Outputs a 8-bit register to the console
    /// </summary>
    private void OutputFullRegisterValue(BitArray registerValue, int left, int top)
    {
        // As binary 'LEDs'
        WriteAtPosition(ToLEDs(registerValue), left, top, 15);

        // As unsigned Hex
        WriteAtPosition("0x" + registerValue.ToString(NumberFormat.UnsignedHexadecimal),
            left, top + 1, 4);

        // As signed decimal
        WriteAtPosition(registerValue.ToString(NumberFormat.SignedDecimal).PadLeft(4),
            left + 6, top + 1, 4);

        // As unsigned decimal
        WriteAtPosition(registerValue.ToString(NumberFormat.UnsignedDecimal).PadLeft(3),
            left + 12, top + 1, 3);
    }

    /// <summary>
    /// Outputs a 4-bit register to the console
    /// </summary>
    private void OutputHalfRegisterValue(BitArray registerValue, int left, int top)
    {
        var halfRegister = new BitArray(registerValue.AsEnumerable().Take(4));

        // As binary 'LEDs'
        WriteAtPosition(ToLEDs(halfRegister), left, top, 7);

        // As unsigned Hex
        WriteAtPosition("0x" + halfRegister.ToString(NumberFormat.UnsignedHexadecimal),
            left, top + 1, 3);

        // As unsigned decimal
        WriteAtPosition(halfRegister.ToString(NumberFormat.UnsignedDecimal).PadLeft(2),
            left + 5, top + 1, 2);
    }


    private void WriteAtPosition(string text, int left, int top, int length)
    {
        Console.SetCursorPosition(left, top);
        var adjustedText = text.Length <= length
            ? text.PadRight(length) : Truncate(text, length); 
        Console.Write(adjustedText);
    }

    private void OnCancel(object? sender, ConsoleCancelEventArgs e)
    {
        e.Cancel = true;
        haltRequested = true;
    }

    private string ToLEDs(BitArray ba)
    {
        return string.Join(' ', ba.AsEnumerable().Reverse().Select(ToLED));
    }

    private static char ToLED(bool b)
    {
        return b ? '●' : '◌';
    }

    string Truncate(string s, int length)
    {
        if (s.Length < length)
            return s;

        return s.Substring(0, length - 1) + "…";
    }

    private static string ToMnemonic(byte opcode)
    {
        return Enum.GetName((BE801Computer.Opcodes)opcode) ?? "???";
    }

    public void Dispose()
    {
        Console.CancelKeyPress -= OnCancel;
        Console.CursorVisible = true;
    }
}
