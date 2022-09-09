using DigitalElectronics.Concepts;
using FluentAssertions;
using NUnit.Framework;
using static DigitalElectronics.Computers.BenEater801Computer;

namespace DigitalElectronics.Computers.Tests
{

    /// <summary>
    /// Tests for Ben Eater's 8-bit programmable computer
    /// </summary>
    public class TestBenEater801Computer
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
            var computer = new BenEater801Computer();
            computer.Should().NotBeNull();
        }

        [Test]
        public void OperateComputerToAdd30And12AndOutput()
        {
            var computer = new BenEater801Computer();
            computer.LoadRAM(programDataAdd12And30);

            ResetProgramCounter();
            Run();

            void ResetProgramCounter()
            {
                computer.SetControlSignal(ControlSignal.CE);
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
                computer.SetControlSignal(ControlSignal.CO);
                computer.SetControlSignal(ControlSignal.MI);
                computer.Clock();

                computer.SetControlSignal(ControlSignal.RO);
                computer.SetControlSignal(ControlSignal.II);
                computer.SetControlSignal(ControlSignal.CE);
                computer.Clock();
            }

            void LDA()
            {
                computer.SetControlSignal(ControlSignal.IO);
                computer.SetControlSignal(ControlSignal.MI);
                computer.Clock();

                computer.SetControlSignal(ControlSignal.RO);
                computer.SetControlSignal(ControlSignal.AI);
                computer.Clock();
            }

            void ADD()
            {
                computer.SetControlSignal(ControlSignal.IO);
                computer.SetControlSignal(ControlSignal.MI);
                computer.Clock();

                computer.SetControlSignal(ControlSignal.RO);
                computer.SetControlSignal(ControlSignal.BI);
                computer.Clock();

                computer.SetControlSignal(ControlSignal.EO);
                computer.SetControlSignal(ControlSignal.AI);
                computer.Clock();
            }

            void OUT()
            {
                computer.SetControlSignal(ControlSignal.AO);
                computer.SetControlSignal(ControlSignal.OI);
                computer.Clock();
            }
        }
    }
}
