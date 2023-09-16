using Marten;
using Microsoft.AspNetCore.Mvc;
using SelfDevelopmentCQRS.ApiModels;

namespace SelfDevelopmentCQRS
{
    public static class CarApiMetods
    {
        public static async Task<IResult> CreateCar([FromServices] IDocumentSession session, [FromBody] CreateCarRequest model)
        {
            var guidId = Guid.NewGuid();
            var carAddEvent = new CreateCar(
                guidId,
                model.Brand, 
                model.Model, 
                model.DateRegistered, 
                model.Description, 
                (EngineType)Enum.Parse(typeof(EngineType), model.EngineType),
                (TransmissionType)Enum.Parse(typeof(TransmissionType), model.TransmissionType),
                new Guid(Constants.UserId)
                );

            var resutStreamExists = session.Events.StartStream(guidId, carAddEvent);

            await session.SaveChangesAsync();

            return Results.Ok(resutStreamExists.Id);
        }


        //01882533-983e-4ef8-affe-021edafccad9
        public static async Task<IResult> GetCar([FromServices] IQuerySession session, [FromRoute] string id)
        {
            var result = await session.Events.AggregateStreamAsync<CarProject>(new Guid(id));

            var apiResult = new CarResponse
            {
                Brand = result.BrandName,
                Model = result.ModelName,
                DateRegistered = result.DateRegistered.ToShortDateString(),
                Description = result.Description,
                EngineType = result.EngineType.ToString(),
                TransmissionType = result.TransmissionType.ToString()
            };

            return Results.Ok(apiResult);
        }
    }
}
