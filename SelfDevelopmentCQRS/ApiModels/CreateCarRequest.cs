namespace SelfDevelopmentCQRS.ApiModels
{
    public class CreateCarRequest
    {
        public string Brand { get; set; }

        public string Model { get; set; }

        public DateTime DateRegistered { get; set; }

        public string TransmissionType { get; set; }

        public string EngineType { get; set; }

        public string Description { get; set; }
    }
}
