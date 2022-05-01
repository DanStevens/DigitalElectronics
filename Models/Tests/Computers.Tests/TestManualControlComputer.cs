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

            // Load the contents of memory address 0x0 (0) in to A Register
            LDA(Address0x0);

            computer.ARegister.ProbeState().ToByte().Should().Be(30);
            computer.BRegister.ProbeState().ToByte().Should().Be(255);

            // Add the contents of memory address 0x1 (1) to the A Register
            ADD(Address0x1);

            computer.BRegister.ProbeState().ToByte().Should().Be(12);
            computer.ARegister.ProbeState().ToByte().Should().Be(42);

            // Put the contents of A Register into the Output Register
            OUT();

            computer.OutRegister.ProbeState().ToByte().Should().Be(42);

            void InitializeRAM()
            {
                computer.RAM.SetInputL(true);

                computer.RAM.SetInputA(Address0x0);
                computer.RAM.SetInputD(new BitArray((byte) 30));
                computer.Clock();
                computer.RAM.ProbeState(Address0x0).ToByte().Should().Be(30);

                computer.RAM.SetInputA(Address0x1);
                computer.RAM.SetInputD(new BitArray((byte) 12));
                computer.RAM.Clock();
                computer.RAM.ProbeState(Address0x1).ToByte().Should().Be(12);

                computer.RAM.SetInputL(false);
            }

            void LDA(BitArray address)
            {
                computer.RAM.SetInputA(address);
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
                computer.RAM.SetInputA(address);
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
        }


    }
}
