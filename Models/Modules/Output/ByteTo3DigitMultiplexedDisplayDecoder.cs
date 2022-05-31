using DigitalElectronics.Concepts;
using DigitalElectronics.Modules.Memory;

namespace DigitalElectronics.Modules.Output
{

    /// <summary>
    /// Models an unsigned byte to 3 digit decimal multiplexed display decoder
    /// </summary>
    /// <remarks>This module decodes a unsigned 8-bit binary input, into a 4 digit
    /// decimal for use with a multiplexed 7-segment display. Multiplexed means
    /// only outputs the segment lines for one of the four digits at a time,
    /// and cycles between them upon each call to <see cref="Clock()"/></remarks>
    public class ByteTo3DigitMultiplexedDisplayDecoder
    {
        private ROM _decoderROM = new (new byte[] { 0x3F, 0x06, 0x5B, 0x4F, 0x66, 0x6D, 0x7D, 0x07, 0x7F, 0x6F });

        public ByteTo3DigitMultiplexedDisplayDecoder()
        {
            _decoderROM.SetInputE(true);
        }

        public void SetInput(BitArray lines)
        {
            _decoderROM.SetInputA(lines);
        }

        public BitArray Output => _decoderROM.Output;
    }
}
