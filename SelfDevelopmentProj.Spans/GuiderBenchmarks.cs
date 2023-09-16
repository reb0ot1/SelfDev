using BenchmarkDotNet.Attributes;

namespace SelfDevelopmentProj.Spans
{
    [MemoryDiagnoser(false)]
    public class GuiderBenchmarks
    {
        private static readonly Guid TestGuid = Guid.Parse("f8a5f468-bd39-44e0-a2e8-dba83d03ef5c");
        private const string TestIdAsString = "aPSl_Dm94ESi6NuoPQPvXA";

        [Benchmark]
        public Guid ToGuidFromString()
        {
            return Guider.ToGuidFromString(TestIdAsString);
        }

        [Benchmark]
        public string ToStringFromGuid()
        {
            return Guider.ToStringFromGuid(TestGuid);
        }
    }
}
