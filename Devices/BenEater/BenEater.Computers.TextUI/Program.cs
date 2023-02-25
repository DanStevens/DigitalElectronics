using System.Text;
using BenEater.Computers.TextUI;
using DigitalElectronics.BenEater.Computers;

var programFile = args.FirstOrDefault(a => !a.StartsWith("/"));

if (programFile == null)
{
    Console.Error.WriteLine("Error: missing argument for program to run");
    Console.WriteLine("Usage: {0} <program> [/turbo]", AppDomain.CurrentDomain.FriendlyName);
    return 255;
}

var program = File.ReadAllBytes(programFile);
var be801Computer = new BE801Computer();
be801Computer.LoadRAM(program);

using var textView = new BE801ComputerTextView(be801Computer)
{
    Turbo = args.Any(a => a.ToUpper() == "/TURBO"),
    ProgramName = Path.GetFileNameWithoutExtension(programFile)
};
textView.Run();
return 0;
