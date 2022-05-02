using DigitalElectronics.Concepts;
using FluentAssertions;
using NUnit.Framework;

namespace DigitalElectronics.Computers.Tests
{

    /// <summary>
    /// Tests for the 8-bit manual control computer
    /// </summary>
    public class TestManualControlComputer
    {
        private readonly BitArray Address0x0 = new BitArray((byte)0x0).Trim(4);
        private readonly BitArray Address0x1 = new BitArray((byte)0x1).Trim(4);
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

            // Initialize RAM
            InitializeRAM();

            // Load the contents of memory address 14 (0xR) in to A Register
            LDA(Address0xE);

            computer.ARegister.ProbeState().ToByte().Should().Be(30);
            computer.BRegister.ProbeState().ToByte().Should().Be(255);

            // Add the contents of memory address 15 (0xF) to the A Register
            ADD(Address0xF);

            computer.BRegister.ProbeState().ToByte().Should().Be(12);
            computer.ARegister.ProbeState().ToByte().Should().Be(42);

            // Put the contents of A Register into the Output Register
            OUT();

            computer.OutRegister.ProbeState().ToByte().Should().Be(42);

            void InitializeRAM()
            {
                // Load the instruction 'LDA 14' (0x1E) int memory address
                WriteMemoryAddress(Address0x0, 0x1D);

                // Load the instruction 'LDA 15' (0x1F) int memory address
                WriteMemoryAddress(Address0x1, 0x1E);

                // Load decimal value 30 into address 14
                WriteMemoryAddress(Address0xE, 30);

                // Load decimal value 12 into address 15
                WriteMemoryAddress(Address0xF, 12);

                void WriteMemoryAddress(BitArray address, byte data)
                {
                    SetAddress(address);

                    computer.RAM.SetInputLD(true);
                    computer.RAM.SetInputD(new BitArray(data));
                    computer.Clock();
                    computer.RAM.SetInputLD(false);
                    computer.RAM.ProbeState(address).ToByte().Should().Be(data);
                }
            }

            void LDA(BitArray address)
            {
                SetAddress(address);
                
                computer.RAM.SetInputE(true);
                
                computer.ARegister.SetInputL(true);
                computer.Clock();
                computer.ALU.SetInputA(computer.ARegister.ProbeState());
                computer.ARegister.SetInputL(false);

                computer.RAM.SetInputE(false);
            }

            void ADD(BitArray address)
            {
                LDB(address);

                computer.ALU.SetInputEO(true);

                computer.ARegister.SetInputL(true);
                computer.Clock();
                computer.ARegister.SetInputL(false);

                computer.ALU.SetInputEO(false);
            }

            void LDB(BitArray address)
            {
                SetAddress(address);

                computer.RAM.SetInputE(true);

                computer.BRegister.SetInputL(true);
                computer.Clock();
                computer.BRegister.SetInputL(false);
                computer.ALU.SetInputB(computer.BRegister.ProbeState());

                computer.RAM.SetInputE(false);
            }

            void OUT()
            {
                computer.ARegister.SetInputE(true);

                computer.OutRegister.SetInputL(true);
                computer.Clock();
                computer.OutRegister.SetInputL(false);

                computer.ARegister.SetInputE(false);
            }

            void SetAddress(BitArray address)
            {
                computer.MAR.SetInputLA(true);
                computer.MAR.SetInputS(address);
                computer.Clock();
                computer.MAR.SetInputLA(false);
            }
        }
    }
}
