namespace SelfDevelopmentCQRS
{
    public static class RegisterApisExtension
    {
        public static void RegisterTestSecond(this WebApplication app)
        {
            app.MapGet("/test2", () => {
                return "test2";
            });
        }
    }
}
