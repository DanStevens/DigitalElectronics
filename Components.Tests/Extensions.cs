using DigitalElectronics.Components.FlipFlops;
using FluentAssertions;

namespace DigitalElectronics.Components
{
    internal static class Extensions
    {
        public static void AssertOutputs(this IOutputsQAndNQ o, bool outputQExpected, bool outputNQExpected)
        {
            o.OutputQ.Should().Be(outputQExpected);
            o.OutputNQ.Should().Be(outputNQExpected);
        }
    }
}
