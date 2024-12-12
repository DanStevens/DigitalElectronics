using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Benchmarking;
using Perfolizer.Horology;

#if RELEASE

var quickJob = Job.Default
    .WithIterationTime(new TimeInterval(10, TimeUnit.Millisecond))
    .WithIterationCount(1)
    .WithWarmupCount(1)
    .WithIterationCount(1);

var config = DefaultConfig.Instance
    .AddJob(Job.Default)
    //.AddJob(quickJob.WithRuntime(CoreRuntime.Core80))
    //.AddJob(quickJob.WithRuntime(CoreRuntime.Core90))
;

BenchmarkRunner.Run<BE801ComputerBenchmarks>();
//BenchmarkRunner.Run<BitArrayInstantiationBenchmarks>(config);

#else

var benchmark = new BE801ComputerBenchmarks();
benchmark.Setup();
benchmark.Clock(1);

#endif
