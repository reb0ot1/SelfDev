using BenchmarkDotNet.Running;
using SelfDevelopmentProj.LINQ;
using System.Numerics;

var vec = new Vector<int>();


BenchmarkRunner.Run<Benchmarks>();