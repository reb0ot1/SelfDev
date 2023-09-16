namespace SelfDevelopmentCQRS
{
    public class RegisterMinApisDI
    {
        public RegisterMinApisDI()
        {

        }

        public void RegisterTestEndpointFirst(WebApplication app)
        {
            app.MapGet("/test1", () =>
            {
                return "test";
            });
        }
    }
}
