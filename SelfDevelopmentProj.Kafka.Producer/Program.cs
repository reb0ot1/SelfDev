using Confluent.Kafka;

var config = new ProducerConfig { BootstrapServers = "localhost:29092,localhost:39092" };

using var producer = new ProducerBuilder<Null, string>(config).Build();

try
{
    string? state;

    while ((state = Console.ReadLine()) is not null)
    {
        var response = await producer.ProduceAsync("weather-topic", new Message<Null, string> { Value = state });
    }
}
catch (Exception ex)
{
	Console.WriteLine("Failed to produce message.");
}