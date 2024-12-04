using BenchmarkDotNet.Running;
using Benchmarking;

#if RELEASE

BenchmarkRunner.Run<BE801ComputerBenchmarks>();

#else

var benchmark = new BE801ComputerBenchmarks();
benchmark.Setup();
benchmark.Clock(1);

#endif
