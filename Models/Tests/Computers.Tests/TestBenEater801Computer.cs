﻿using System.Linq;
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

        // Machine code and data for test ProgramComputerToOutput3TimesTable
        private readonly byte[] programData3TimesTable =
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
            var computer = new BenEater801Computer();
            computer.Should().NotBeNull();
        }

        /// <summary>
        /// Tests the computer by manually operating the control signals of the computer
        /// </summary>
        [Test]
        public void OperateComputerToAdd30And12AndOutput()
        {
            var computer = new BenEater801Computer() {  ManualControlMode = true };
            computer.LoadRAM(programDataAdd12And30);

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

        [Test]
        public void ProgramComputerToOutput3TimesTable()
        {
            var computer = new BenEater801Computer() {  ManualControlMode = false };
            computer.LoadRAM(programData3TimesTable);

            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(15);
            computer.ProbeMAR().ToByte().Should().Be(0);
            computer.ProbePC().ToByte().Should().Be(0);
            computer.ProbeOutRegister().ToByte().Should().Be(255);

            // LDI 3
            computer.Clock(); // Step 0 - MI|CO
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(0);
            computer.ProbePC().ToByte().Should().Be(0);
            computer.Clock(); // Step 1 - RO|II|CE
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(1);
            computer.ProbePC().ToByte().Should().Be(1);
            computer.ProbeInstrRegister().ToByte().Should().Be(0b0101_0011);
            computer.Clock(); // Step 2 - IO|AI
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(2);
            computer.ProbeARegister().ToByte().Should().Be(3);
            computer.Clock(); // Step 3 - (no op)
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(3);
            computer.Clock(); // Step 4 - (no op)
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(4);
            computer.Clock(); // Step 5 - (no op)
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(5);

            // STA 15
            computer.Clock(); // Step 0 - MI|CO
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(0);
            computer.ProbePC().ToByte().Should().Be(1);
            computer.Clock(); // Step 1 - RO|II|CE
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(1);
            computer.ProbePC().ToByte().Should().Be(2);
            computer.ProbeInstrRegister().ToByte().Should().Be(0b0100_1111);
            computer.Clock(); // Step 2 - IO|MI
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(2);
            computer.Clock(); // Step 3 - AO|RI
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(3);
            computer.ProbeRAM(new BitArray((byte)15)).ToByte().Should().Be(3);
            computer.Clock(); // Step 4 - (no op)
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(4);
            computer.Clock(); // Step 5 - (no op)
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(5);

            // LDI 0
            computer.Clock(); // Step 0 - MI|CO
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(0);
            computer.ProbePC().ToByte().Should().Be(2);
            computer.Clock(); // Step 1 - RO|II|CE
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(1);
            computer.ProbePC().ToByte().Should().Be(3);
            computer.ProbeInstrRegister().ToByte().Should().Be(0b0101_0000);
            computer.Clock(); // Step 2 - IO|AI
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(2);
            computer.ProbeARegister().ToByte().Should().Be(0);
            computer.Clock(); // Step 3 - (no op)
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(3);
            computer.Clock(); // Step 4 - (no op)
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(4);
            computer.Clock(); // Step 5 - (no op)
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(5);

            // ADD 15
            computer.Clock(); // Step 0 - MI|CO
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(0);
            computer.ProbePC().ToByte().Should().Be(3);
            computer.Clock(); // Step 1 - RO|II|CE
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(1);
            computer.ProbePC().ToByte().Should().Be(4);
            computer.Clock(); // Step 2 - RO|BI
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(2);
            computer.ProbeInstrRegister().ToByte().Should().Be(0b0010_1111);
            computer.Clock(); // Step 3 - EO|AI
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(3);
            computer.ProbeBRegister().ToByte().Should().Be(3);
            computer.Clock(); // Step 4 - (no op)
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(4);
            computer.ProbeARegister().ToByte().Should().Be(3);
            computer.Clock(); // Step 5 - (no op)
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(5);

            // OUT
            computer.Clock(); // Step 0 - MI|CO
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(0);
            computer.ProbePC().ToByte().Should().Be(4);
            computer.Clock(); // Step 1 - RO|II|CE
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(1);
            computer.ProbePC().ToByte().Should().Be(5);
            computer.Clock(); // Step 2 - AO|OI
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(2);
            computer.ProbeInstrRegister().ToByte().Should().Be(0b1110_0000);
            computer.ProbeOutRegister().ToByte().Should().Be(3);
            computer.Clock(); // Step 3 - (no op)
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(3);
            computer.Clock(); // Step 4 - (no op)
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(4);
            computer.Clock(); // Step 5 - (no op)
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(5);

            // JMP 3
            computer.Clock(); // Step 0 - MI|CO
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(0);
            computer.ProbePC().ToByte().Should().Be(5);
            computer.Clock(); // Step 1 - RO|II|CE
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(1);
            computer.ProbePC().ToByte().Should().Be(6);
            computer.Clock(); // Step 2 - IO|J
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(2);
            computer.ProbePC().ToByte().Should().Be(3);
            computer.Clock(); // Step 3 - (no op)
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(3);
            computer.Clock(); // Step 4 - (no op)
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(4);
            computer.Clock(); // Step 5 - (no op)
            computer.ProbeMicroinstrStepCounter().ToByte().Should().Be(5);
        }
    }
}
