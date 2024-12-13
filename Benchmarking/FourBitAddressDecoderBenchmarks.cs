using System;
using DigitalElectronics.Concepts;
using DigitalElectronics.Modules.Memory;

namespace Benchmarking;

[MemoryDiagnoser]
public class FourBitAddressDecoderBenchmarks
{
    private FourBitAddressDecoder decoder = new();

    [Benchmark] // Does not allocate
    public BitArray OutputY()
    {
        return decoder.OutputY;
    }

    [Benchmark] // Does not allocate
    public void SetInputA()
    {
        decoder.SetInputA(new BitArray(0, length: 4));
    }
}
