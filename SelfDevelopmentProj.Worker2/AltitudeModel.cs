namespace SelfDevelopmentProj.Worker2
{
    internal class AltitudeModel
    {
        public string Time { get; init; }
        public int Altitude { get; init; }

        public override string ToString()
        {
            return $"Plane was at altitude {Altitude} ft. at {Time}.";
        }
    }
}
