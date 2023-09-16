using Marten;
using Microsoft.AspNetCore.Mvc;

namespace SelfDevelopmentCQRS
{
    public static class ModelApiMethods
    {
        public static async Task<IResult> UpdateModel([FromServices] IDocumentSession session, [FromRoute] Guid id, [FromBody]NameRequestModel model)
        {
            var resutStreamExists = session.Events.Append(id, new UpdateModelName(model.Name));

            if (resutStreamExists is null)
            {
                return Results.BadRequest("Model not updated.");
            }

            await session.SaveChangesAsync();

            return Results.Ok("Model updated.");
        }
    }
}
