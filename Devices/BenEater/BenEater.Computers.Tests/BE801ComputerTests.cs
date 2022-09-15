using System.Linq;
using DigitalElectronics.Concepts;
using FluentAssertions;
using NUnit.Framework;
using static DigitalElectronics.BenEater.Computers.BE801Computer;

namespace DigitalElectronics.BenEater.Computers.Tests
{

    /// <summary>
    /// Tests for Ben Eater's 8-bit programmable computer
    /// </summary>
    public class BE801ComputerTests
    {

        // Machine code and data for test OperateComputerToAdd30And12AndOutput
        private readonly byte[] machineCodeAdd12And30 =
        {
            0x1E, // LDA 14
            0x1F, // ADD 15
            0xE0, // OUT
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // Unused
            30, 12 // Data
        };

        // Machine code and data for test ProgramComputerToOutput3TimesTable
        private readonly byte[] machineCode3TimesTable =
        {
            0b0101_0011, // LDI 3
            0b0100_1111, // STA 15
            0b0101_0000, // LDI 0
            0b0010_1111, // ADD 15
            0b1110_0000, // OUT
            0b0110_0011, // JMP 3
        };

        [Test]
        public void Create()
        {
            var computer = new BE801Computer();
            computer.Should().NotBeNull();
        }

        /// <summary>
        /// Tests the computer by manually operating the control signals of the computer
        /// (<see cref="ManualControlMode"/> is set) to execute machine code that adds 12 and 30
        /// </summary>
        [Test]
        public void OperateComputerToAdd30And12AndOutput()
        {
            var computer = new BE801Computer() {  ManualControlMode = true };
            computer.LoadRAM(machineCodeAdd12And30);

            Run();

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
                computer.SetControlSignals(ControlSignals.CO);
                computer.SetControlSignals(ControlSignals.MI);
                computer.Clock();

                computer.SetControlSignals(ControlSignals.RO);
                computer.SetControlSignals(ControlSignals.II);
                computer.SetControlSignals(ControlSignals.CE);
                computer.Clock();
            }

            void LDA()
            {
                computer.SetControlSignals(ControlSignals.IO);
                computer.SetControlSignals(ControlSignals.MI);
                computer.Clock();

                computer.SetControlSignals(ControlSignals.RO);
                computer.SetControlSignals(ControlSignals.AI);
                computer.Clock();
            }

            void ADD()
            {
                computer.SetControlSignals(ControlSignals.IO);
                computer.SetControlSignals(ControlSignals.MI);
                computer.Clock();

                computer.SetControlSignals(ControlSignals.RO);
                computer.SetControlSignals(ControlSignals.BI);
                computer.Clock();

                computer.SetControlSignals(ControlSignals.EO);
                computer.SetControlSignals(ControlSignals.AI);
                computer.Clock();
            }

            void OUT()
            {
                computer.SetControlSignals(ControlSignals.AO);
                computer.SetControlSignals(ControlSignals.OI);
                computer.Clock();
            }
        }

        /// <summary>
        /// Tests the computer by loading the machine code for computing the 3 times
        /// table into memory and calling to <see cref="BE801Computer.Clock"/>
        /// method repeatedly to advance the program
        /// </summary>
        [Test]
        public void ProgramComputerToOutput3TimesTable()
        {
            var computer = new BE801Computer() { ManualControlMode = false };
            computer.LoadRAM(machineCode3TimesTable);
            byte pc = 0;

            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(15);
            computer.ProbeMAR().ToByte().Should().Be(0);
            computer.ProbePC().ToByte().Should().Be(pc);
            computer.ProbeOutRegister().ToByte().Should().Be(255);

            LDI_0x3(expectedARegister: 3);
            STA_0xF(expectedRAMAddr15: 3);
            LDI_0x0(expectedARegister: 0);

            for (byte i = 3; i <= 36; i += 3)
            {
                ADD_0xF(expectedARegister: i, expectedBRegister: 3);
                OUT____(expectedOutRegister: i);
                JMP_0x3(expectedPC: 3);
            }

            void LDI_0x3(byte expectedARegister)
            {
                computer.Clock(); // Step 0 - MI|CO
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(pc);
                computer.ProbePC().ToByte().Should().Be(0);
                computer.Clock(); // Step 1 - RO|II|CE
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(1);
                computer.ProbePC().ToByte().Should().Be(++pc);
                computer.ProbeInstrRegister().ToByte().Should().Be(0b0101_0011);
                computer.Clock(); // Step 2 - IO|AI
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(2);
                computer.ProbeARegister().ToByte().Should().Be(expectedARegister);
                computer.Clock(); // Step 3 - (no op)
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(3);
                computer.Clock(); // Step 4 - (no op)
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(4);
                computer.Clock(); // Step 5 - (no op)
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(5);
            }

            void STA_0xF(byte expectedRAMAddr15)
            {
                computer.Clock(); // Step 0 - MI|CO
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(0);
                computer.ProbePC().ToByte().Should().Be(pc);
                computer.Clock(); // Step 1 - RO|II|CE
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(1);
                computer.ProbePC().ToByte().Should().Be(++pc);
                computer.ProbeInstrRegister().ToByte().Should().Be(0b0100_1111);
                computer.Clock(); // Step 2 - IO|MI
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(2);
                computer.Clock(); // Step 3 - AO|RI
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(3);
                computer.ProbeRAM(new BitArray((byte)15)).ToByte().Should().Be(expectedRAMAddr15);
                computer.Clock(); // Step 4 - (no op)
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(4);
                computer.Clock(); // Step 5 - (no op)
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(5);
            }

            void LDI_0x0(byte expectedARegister)
            {
                computer.Clock(); // Step 0 - MI|CO
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(0);
                computer.ProbePC().ToByte().Should().Be(pc);
                computer.Clock(); // Step 1 - RO|II|CE
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(1);
                computer.ProbePC().ToByte().Should().Be(++pc);
                computer.ProbeInstrRegister().ToByte().Should().Be(0b0101_0000);
                computer.Clock(); // Step 2 - IO|AI
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(2);
                computer.ProbeARegister().ToByte().Should().Be(expectedARegister);
                computer.Clock(); // Step 3 - (no op)
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(3);
                computer.Clock(); // Step 4 - (no op)
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(4);
                computer.Clock(); // Step 5 - (no op)
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(5);
            }

            void ADD_0xF(byte expectedARegister, byte expectedBRegister)
            {
                computer.Clock(); // Step 0 - MI|CO
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(0);
                computer.ProbePC().ToByte().Should().Be(pc);
                computer.Clock(); // Step 1 - RO|II|CE
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(1);
                computer.ProbePC().ToByte().Should().Be(++pc);
                computer.Clock(); // Step 2 - RO|BI
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(2);
                computer.ProbeInstrRegister().ToByte().Should().Be(0b0010_1111);
                computer.Clock(); // Step 3 - EO|AI
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(3);
                computer.ProbeBRegister().ToByte().Should().Be(expectedBRegister);
                computer.Clock(); // Step 4 - (no op)
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(4);
                computer.ProbeARegister().ToByte().Should().Be(expectedARegister);
                computer.Clock(); // Step 5 - (no op)
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(5);
            }

            void OUT____(byte expectedOutRegister)
            {
                computer.Clock(); // Step 0 - MI|CO
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(0);
                computer.ProbePC().ToByte().Should().Be(pc);
                computer.Clock(); // Step 1 - RO|II|CE
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(1);
                computer.ProbePC().ToByte().Should().Be(++pc);
                computer.Clock(); // Step 2 - AO|OI
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(2);
                computer.ProbeInstrRegister().ToByte().Should().Be(0b1110_0000);
                computer.ProbeOutRegister().ToByte().Should().Be(expectedOutRegister);
                computer.Clock(); // Step 3 - (no op)
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(3);
                computer.Clock(); // Step 4 - (no op)
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(4);
                computer.Clock(); // Step 5 - (no op)
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(5);
            }

            void JMP_0x3(byte expectedPC)
            {
                computer.Clock(); // Step 0 - MI|CO
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(0);
                computer.ProbePC().ToByte().Should().Be(pc);
                computer.Clock(); // Step 1 - RO|II|CE
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(1);
                computer.ProbePC().ToByte().Should().Be(++pc);
                computer.Clock(); // Step 2 - IO|J
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(2);
                computer.ProbePC().ToByte().Should().Be(expectedPC);
                pc = computer.ProbePC().ToByte();
                computer.Clock(); // Step 3 - (no op)
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(3);
                computer.Clock(); // Step 4 - (no op)
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(4);
                computer.Clock(); // Step 5 - (no op)
                computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(5);
            }
        }
    }
}
