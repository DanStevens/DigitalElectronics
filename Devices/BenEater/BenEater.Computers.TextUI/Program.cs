using System.Text;
using BenEater.Computers.TextUI;
using DigitalElectronics.BenEater.Computers;

var program = File.ReadAllBytes(args[0]);
var be801Computer = new BE801Computer();
be801Computer.LoadRAM(program);

var textView = new BE801ComputerTextView(be801Computer);
textView.Turbo = true;
textView.Run();
