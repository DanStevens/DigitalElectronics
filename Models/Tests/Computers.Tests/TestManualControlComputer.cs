using DigitalElectronics.Concepts;
using FluentAssertions;
using NUnit.Framework;
using static DigitalElectronics.Computers.ManualControlComputer;

namespace DigitalElectronics.Computers.Tests
{

    /// <summary>
    /// Tests for the 8-bit manual control computer
    /// </summary>
    public class TestManualControlComputer
    {

        // Machine code and data for test OperateComputerToAdd30And12AndOutput
        private readonly byte[] programDataAdd12And30 =
        {
            0x1E, // LDA 14
            0x1F, // ADD 15
            0xE0, // OUT
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // Unused
            30, 12 // Data
        };

        [Test]
        public void Create()
        {
            var computer = new ManualControlComputer();
            computer.Should().NotBeNull();
        }

        [Test]
        public void OperateComputerToAdd30And12AndOutput()
        {
            var computer = new ManualControlComputer();
            computer.LoadRAM(programDataAdd12And30);

            ResetProgramCounter();
            Run();

            void ResetProgramCounter()
            {
                computer.SetControlSignal(ControlWord.CE);
                computer.Clock();
                computer.ProbePC().ToByte().Should().Be(0);
            }

            void Run()
            {
                FetchInstruction();

                computer.ProbePC().ToByte().Should().Be(1);
                computer.ProbeInstrRegister().ToByte().Should().Be(0x1E);

                // Load the contents of memory address 14 (0xE) in to A Register
                LDA();

                //computer.ProbeIRegister().ToByte().Should().Be(0xE);
                computer.ProbeMAR().ToByte().Should().Be(0xE);
                computer.ProbeARegister().ToByte().Should().Be(30);
                computer.ProbeBRegister().ToByte().Should().Be(255);

                FetchInstruction();

                computer.ProbePC().ToByte().Should().Be(2);
                computer.ProbeInstrRegister().ToByte().Should().Be(0x1F);

                // Add the contents of memory address 15 (0xF) to the A Register
                ADD();

                computer.ProbeBRegister().ToByte().Should().Be(12);
                computer.ProbeARegister().ToByte().Should().Be(42);

                FetchInstruction();

                computer.ProbePC().ToByte().Should().Be(3);
                computer.ProbeInstrRegister().ToByte().Should().Be(0xE0);

                // Put the contents of A Register into the Output Register
                OUT();

                computer.ProbeOutRegister().ToByte().Should().Be(42);
            }

            void FetchInstruction()
            {
                computer.SetControlSignal(ControlWord.CO);
                computer.SetControlSignal(ControlWord.MI);
                computer.Clock();

                computer.SetControlSignal(ControlWord.RO);
                computer.SetControlSignal(ControlWord.II);
                computer.SetControlSignal(ControlWord.CE);
                computer.Clock();
            }

            void LDA()
            {
                computer.SetControlSignal(ControlWord.IO);
                computer.SetControlSignal(ControlWord.MI);
                computer.Clock();

                computer.SetControlSignal(ControlWord.RO);
                computer.SetControlSignal(ControlWord.AI);
                computer.Clock();
            }

            void ADD()
            {
                computer.SetControlSignal(ControlWord.IO);
                computer.SetControlSignal(ControlWord.MI);
                computer.Clock();

                computer.SetControlSignal(ControlWord.RO);
                computer.SetControlSignal(ControlWord.BI);
                computer.Clock();

                computer.SetControlSignal(ControlWord.EO);
                computer.SetControlSignal(ControlWord.AI);
                computer.Clock();
            }

            void OUT()
            {
                computer.SetControlSignal(ControlWord.AO);
                computer.SetControlSignal(ControlWord.OI);
                computer.Clock();
            }
        }
    }
}
