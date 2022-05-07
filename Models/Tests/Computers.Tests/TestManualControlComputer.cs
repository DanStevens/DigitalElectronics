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
            computer.PC.ProbeState().ToByte().Should().Be(0xF);

            // Initialize RAM
            InitializeRAM();

            FetchInstruction();

            computer.PC.ProbeState().ToByte().Should().Be(0);
            computer.IRegister.ProbeState().ToByte().Should().Be(0x1E);

            // Load the contents of memory address 14 (0xE) in to A Register
            LDA();

            //computer.IRegister.ProbeState().ToByte().Should().Be(0xE);
            computer.MAR.ProbeAddress().ToByte().Should().Be(0xE);
            computer.ARegister.ProbeState().ToByte().Should().Be(30);
            computer.BRegister.ProbeState().ToByte().Should().Be(255);

            FetchInstruction();

            computer.PC.ProbeState().ToByte().Should().Be(1);
            computer.IRegister.ProbeState().ToByte().Should().Be(0x1F);

            // Add the contents of memory address 15 (0xF) to the A Register
            ADD();

            computer.BRegister.ProbeState().ToByte().Should().Be(12);
            computer.ARegister.ProbeState().ToByte().Should().Be(42);

            FetchInstruction();

            computer.PC.ProbeState().ToByte().Should().Be(2);
            computer.IRegister.ProbeState().ToByte().Should().Be(0xE0);

            // Put the contents of A Register into the Output Register
            OUT();

            computer.OutRegister.ProbeState().ToByte().Should().Be(42);

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

                    computer.RAM.SetInputLD(true);
                    computer.RAM.SetInputD(new BitArray(data));
                    computer.Clock();
                    computer.RAM.ProbeState(address).ToByte().Should().Be(data);
                }
            }

            // TODO: See if this can be reduced to 2 cycles
            void FetchInstruction()
            {
                computer.PC.SetInputCE(true);  // Set signal CE high
                computer.Clock();
                
                computer.PC.SetInputE(true);   // Set signal CO high
                computer.MAR.SetInputLA(true); // Set signal MI high
                computer.Clock();

                computer.RAM.SetInputE(true);       // Set signal RO high
                computer.IRegister.SetInputL(true); // Set signal II high
                computer.Clock();
            }

            void LDA()
            {
                computer.IRegister.SetInputE(true); // Set signal IO high
                computer.MAR.SetInputLA(true);      // Set signal MI high
                computer.Clock();

                computer.RAM.SetInputE(true);       // Set signal RO high
                computer.ARegister.SetInputL(true); // Set signal AI high
                computer.Clock();
            }

            void ADD()
            {
                computer.IRegister.SetInputE(true); // Set signal IO high
                computer.MAR.SetInputLA(true);      // Set signal MI high
                computer.Clock();

                computer.RAM.SetInputE(true);       // Set signal RO high
                computer.BRegister.SetInputL(true); // Set signal BI high
                computer.Clock();

                computer.ALU.SetInputEO(true);      // Set signal EO high
                computer.ARegister.SetInputL(true); // Set signal AI high
                computer.Clock();
            }

            void OUT()
            {
                computer.ARegister.SetInputE(true);   // Set signal AO high
                computer.OutRegister.SetInputL(true); // Set signal OI high
                computer.Clock();
            }

            void SetAddress(BitArray address)
            {
                computer.MAR.SetInputLA(true);
                computer.MAR.SetInputS(address);
                computer.Clock();
            }
        }
    }
}
