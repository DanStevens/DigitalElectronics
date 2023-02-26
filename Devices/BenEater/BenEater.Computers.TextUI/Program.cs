using System.Text;
using BenEater.Computers.TextUI;
using DigitalElectronics.BenEater.Computers;

var programFile = args.FirstOrDefault(a => !a.StartsWith("/"));

if (programFile == null)
{
    OutputErrorMessageAndUsage("Missing <program> parameter");
    return 255;
}

var program = File.ReadAllBytes(programFile);
var be801Computer = new BE801Computer();
be801Computer.LoadRAM(program);

try
{
    using var textView = new BE801ComputerTextView(be801Computer)
    {
        Turbo = args.Any(a => a.ToUpper() == "/TURBO"),
        ProgramName = Path.GetFileNameWithoutExtension(programFile),
        WaitTimeBetweenClockCycles = ParseWaitArg(args) ?? 50
    };

    textView.Run();
    return 0;
}
catch (CommandLineParameterException ex)
{
    OutputErrorMessageAndUsage(ex.Message);
    return ex.ExitCode;
}

int? ParseWaitArg(string[] strings)
{
    try
    {
        var waitArgs = args.Where(a => a.ToUpper().StartsWith("/WAIT")).ToArray();
        if (!waitArgs.Any())
        {
            return null;
        }

        return int.Parse(waitArgs.Single().Split(":")[1]);

    }
    catch (InvalidOperationException)
    {
        throw new CommandLineParameterException("{0} parameter can only be specified once", "/wait:", 1);
    }
    catch (FormatException)
    {
        throw new CommandLineParameterException("{0} parameter is invalid or missing value", "/wait:", 2);
    }
}

void OutputErrorMessageAndUsage(string message)
{
    Console.Error.WriteLine($"Error: {message}");
    Console.WriteLine(BE801ComputerTextView.Usage);
}

public class CommandLineParameterException : ArgumentException
{
    public int ExitCode { get; set; }

    public CommandLineParameterException(string message, string? paramName, int exitCode = 1)
        : base(string.Format(message, paramName), paramName)
    {
        ExitCode = exitCode;
    }
}
