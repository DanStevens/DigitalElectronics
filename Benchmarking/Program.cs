using BenchmarkDotNet.Running;
using Benchmarking;

#if RELEASE

BenchmarkRunner.Run<BE801ComputerBenchmarks>();

#else

var benchmark = new BE801ComputerBenchmarks();
benchmark.Setup();
benchmark.PerformControlLogic(1);
benchmark.PerformControlLogic(2);
benchmark.PerformControlLogic(3);
benchmark.PerformControlLogic(4);
benchmark.PerformControlLogic(5);
Console.WriteLine("Done");

#endif
