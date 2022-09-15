using DigitalElectronics.Concepts;
using DigitalElectronics.Components.Memory;

namespace DigitalElectronics.BenEater.Computers
{

    public partial class BE801Computer
    {
        /// <summary>
        /// The instruction register for the BE-801 computer
        /// </summary>
        /// <remarks>The instruction register for the BE-801 computer only outputs the first 4 least significant
        /// bits</remarks>
        public class InstructionRegister : IReadWriteRegister
        {
            private readonly IReadWriteRegister innerRegister;

            public InstructionRegister(int wordSize)
            {
                innerRegister = new Register(wordSize) { Label = "Instruction Register" };
            }

            public RegisterMode Mode => innerRegister.Mode;

            public string Label { get => innerRegister.Label; set => innerRegister.Label = value; }

            /// <summary>
            /// The tri-state output of the module
            /// </summary>
            /// <returns>If the output is enabled (see <see cref="SetInputE"/>,
            /// <see cref="BitArray"/> representing the current value; otherwise `null`,
            /// which represents the Z (high impedance) state</returns>
            /// <note>
            /// Only the first 4 least-signification bits are output
            /// </note>
            public BitArray? Output => innerRegister.Output?.And(new BitArray((byte)0b1111));

            public int WordSize => innerRegister.WordSize;

            public void Clock() => innerRegister.Clock();

            public BitArray ProbeState() => innerRegister.ProbeState();

            public void Reset() => innerRegister.Reset();

            public void SetInputD(BitArray data) => innerRegister.SetInputD(data);

            public void SetInputE(bool value) => innerRegister.SetInputE(value);

            public void SetInputL(bool value) => innerRegister.SetInputL(value);
        }
    }
}
