using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Benchmarking;
using DigitalElectronics.Components.Memory;
using Perfolizer.Horology;
using static DigitalElectronics.BenEater.Computers.BE801Computer;

#if RELEASE

var quickJob = Job.Default
    .WithIterationTime(new TimeInterval(10, TimeUnit.Millisecond))
    .WithIterationCount(1)
    .WithWarmupCount(1)
    .WithIterationCount(1);

var config = DefaultConfig.Instance
    //.AddJob(Job.Default)
    //.AddJob(quickJob.WithRuntime(CoreRuntime.Core80))
    .AddJob(quickJob.WithRuntime(CoreRuntime.Core90))
;

BenchmarkRunner.Run<BE801ComputerBenchmarks>(config);
//BenchmarkRunner.Run<BitArrayInstantiationBenchmarks>(config);
//BenchmarkRunner.Run<BinaryCounterBenchmarks>(config);
//BenchmarkRunner.Run<FourBitAddressDecoderBenchmarks>(config);
//BenchmarkRunner.Run<RegisterBenchmarks>(config);
//BenchmarkRunner.Run<RegisterBitBenchmarks>(config);
//BenchmarkRunner.Run<ROMBenchmarks>(config);

#else

var benchmark = new BE801ComputerBenchmarks();
benchmark.Setup();
benchmark.SetControlSignal(ControlSignals.RO);

//var benchmark = new BitArrayInstantiationBenchmarks();
//benchmark.FromEnumerableOf32BoolsAndLength();
//_ = benchmark.ArrayOf32Buffers_ViaToBitArray();

//var benchmark = new RegisterBenchmarks();
//benchmark.SetInputE();

//var benchmark = new RegisterBitBenchmarks();
//benchmark.SetInputE();

//var benchmark = new MiscBenchmarks();
//benchmark.FromEnumerableOf32Bools();

//var benchmark1 = new ROMBenchmarks();
//benchmark1.Setup();
//benchmark1.SetInputA_UsingBitArray();
//var benchmark2 = new ROMBenchmarks();
//benchmark2.Setup();
//benchmark2.SetInputA_IndividualBit();
//var benchmark3 = new ROMBenchmarks();
//benchmark3.Setup();
//benchmark3.Output();

Console.WriteLine("Done");

#endif
