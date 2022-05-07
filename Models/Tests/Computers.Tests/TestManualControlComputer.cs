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
        private readonly BitArray Address0x0 = new BitArray((byte)0x0).Trim(4);
        private readonly BitArray Address0x1 = new BitArray((byte)0x1).Trim(4);
        private readonly BitArray Address0x2 = new BitArray((byte)0x2).Trim(4);
        private readonly BitArray Address0xE = new BitArray((byte)0xE).Trim(4);
        private readonly BitArray Address0xF = new BitArray((byte)0xF).Trim(4);

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
            computer.ProbePC().ToByte().Should().Be(0xF);

            // Initialize RAM
            InitializeRAM();

            FetchInstruction();

            computer.ProbePC().ToByte().Should().Be(0);
            computer.ProbeIRegister().ToByte().Should().Be(0x1E);

            // Load the contents of memory address 14 (0xE) in to A Register
            LDA();

            //computer.ProbeIRegister().ToByte().Should().Be(0xE);
            computer.ProbeMAR().ToByte().Should().Be(0xE);
            computer.ProbeARegister().ToByte().Should().Be(30);
            computer.ProbeBRegister().ToByte().Should().Be(255);

            FetchInstruction();

            computer.ProbePC().ToByte().Should().Be(1);
            computer.ProbeIRegister().ToByte().Should().Be(0x1F);

            // Add the contents of memory address 15 (0xF) to the A Register
            ADD();

            computer.ProbeBRegister().ToByte().Should().Be(12);
            computer.ProbeARegister().ToByte().Should().Be(42);

            FetchInstruction();

            computer.ProbePC().ToByte().Should().Be(2);
            computer.ProbeIRegister().ToByte().Should().Be(0xE0);

            // Put the contents of A Register into the Output Register
            OUT();

            computer.ProbeOutRegister().ToByte().Should().Be(42);

            void InitializeRAM()
            {
                // Load the instruction 'LDA 14' (0x1E) int memory address
                WriteMemoryAddress(Address0x0, 0x1E);

                // Load the instruction 'ADD 15' (0x1F) int memory address
                WriteMemoryAddress(Address0x1, 0x1F);

                // Load the instruction 'OUT' (0x1F) int memory address
                WriteMemoryAddress(Address0x2, 0xE0);

                // Load decimal value 30 into address 14
                WriteMemoryAddress(Address0xE, 30);

                // Load decimal value 12 into address 15
                WriteMemoryAddress(Address0xF, 12);

                void WriteMemoryAddress(BitArray address, byte data)
                {
                    SetAddress(address);

                    computer.SetControlSignal(ControlWord.RI);
                    computer.SetRAM(new BitArray(data));
                    computer.Clock();
                    computer.ProbeRAM(address).ToByte().Should().Be(data);
                }

                void SetAddress(BitArray address)
                {
                    computer.SetControlSignal(ControlWord.MI);
                    computer.SetMAR(address);
                    computer.Clock();
                }
            }

            // TODO: See if this can be reduced to 2 cycles
            void FetchInstruction()
            {
                computer.SetControlSignal(ControlWord.CE);
                computer.Clock();
                
                computer.SetControlSignal(ControlWord.CO);
                computer.SetControlSignal(ControlWord.MI);
                computer.Clock();

                computer.SetControlSignal(ControlWord.RO);
                computer.SetControlSignal(ControlWord.II);
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
