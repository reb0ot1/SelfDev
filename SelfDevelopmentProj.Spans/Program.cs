namespace SelfDevelopmentProj.Spans
{
    public class Program
    {
        static void Main(string[] args)
        {
            //BenchmarkRunner.Run<GuiderBenchmarks>();
            var id = Guid.NewGuid();
            Console.WriteLine(id);

            var base64Id = Convert.ToBase64String(id.ToByteArray());
            Console.WriteLine(base64Id);

            var urlFriendlyBase64Id = Guider.ToStringFromGuid(id);
            Console.WriteLine(urlFriendlyBase64Id);

            var idAgain = Guider.ToGuidFromString(urlFriendlyBase64Id);
            Console.WriteLine(idAgain);
        }
    }
}