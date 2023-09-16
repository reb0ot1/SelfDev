using Confluent.Kafka;

var config = new ConsumerConfig
{
    BootstrapServers = "localhost:29092,localhost:39092",
    GroupId = "weather-consumer-group",
    AutoOffsetReset = AutoOffsetReset.Earliest
};

using var consumer = new ConsumerBuilder<Null, string>(config).Build();

consumer.Subscribe("weather-topic");

CancellationTokenSource tokenSource= new CancellationTokenSource();

try
{
    while (true)
    {
        var response = consumer.Consume(tokenSource.Token);
        if (response is not null)
        {
            Console.WriteLine($"Message received -> {response.Message.Value}");
        }
    }
}
catch (Exception)
{

	throw;
}