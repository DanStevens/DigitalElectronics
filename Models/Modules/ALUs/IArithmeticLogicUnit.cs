using System.Collections;
using BitArray = DigitalElectronics.Concepts.BitArray;

namespace DigitalElectronics.Modules.ALUs
{
    public interface IArithmeticLogicUnit : IModule
    {
        /// <summary>
        /// Gets state of 'Sum' (∑) output
        /// </summary>
        /// <return>The sum of 'A' input and 'B' input</return>
        BitArray? OutputE { get; }

        /// <summary>
        /// Sets value for A input
        /// </summary>
        /// <param name="data">A BitArray representing the value for A input</param>
        void SetInputA(BitArray data);

        /// <summary>
        /// Sets value for B input
        /// </summary>
        /// <param name="data">A BitArray representing the value for B input</param>
        void SetInputB(BitArray data);

        /// <summary>
        /// Sets the value for 'Sum output' signal
        /// </summary>
        /// <param name="value">Set to `true` to enable the 'Sum' output and `false`
        /// to disable the 'Sum' output</param>
        void SetInputEO(bool value);

        /// <summary>
        /// Sets the value for the 'Subtract' signal
        /// </summary>
        /// <param name="value">Set to `true` to set ALU to subtraction mode and `false` for
        /// addition mode</param>
        void SetInputSu(bool value);

        /// <summary>
        /// Returns the internal state of the ALU
        /// </summary>
        /// <remarks>Consumers can use this to get the ALU's sum output without have to set
        /// the 'Sum Output' signal (<see cref="ArithmeticLogicUnit.SetInputEO"/>) to `true`.</remarks>
        BitArray ProbeState();
    }
}
