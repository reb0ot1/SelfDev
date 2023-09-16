using Microsoft.AspNetCore.Mvc;

namespace SelfDevelopmentCQRS
{
    public static class ApiMethods
    {
        public async static Task<IResult> TestMethod([FromServices]HttpClient httpClient)
        {
            await httpClient.GetAsync("https://google.bg");
            return Results.Ok("test4");
        }
    }
}
