namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var idGuidString = "Hello world.";
            var ttt = idGuidString.Replace("-", "_");

            Console.WriteLine(ttt);
        }
    }
}