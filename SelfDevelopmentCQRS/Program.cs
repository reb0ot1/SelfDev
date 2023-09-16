using SelfDevelopmentCQRS;
using Marten;
using Microsoft.AspNetCore.Mvc;
using Marten.Events.Projections;
using SelfDevelopmentCQRS.ApiModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddScoped<RegisterMinApisDI>();

builder.Services.AddMarten(o =>
{
    o.Connection(builder.Configuration.GetConnectionString("Default"));
    o.Projections.Add<CarSummaryProjector>(Marten.Events.Projections.ProjectionLifecycle.Inline);
    //o.Projections.Add<ModelsProjector>(Marten.Events.Projections.ProjectionLifecycle.Inline);
});

var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseHttpsRedirection();

app.MapGet("/test3", ApiMethods.TestMethod);

app.MapGet("/rebuild", async (IDocumentStore store) =>
{
    var daemon = await store.BuildProjectionDaemonAsync();
    await daemon.RebuildProjection<CarSummary>(CancellationToken.None);
    return "rebuilt!";
});

app.MapPost("/car", CarApiMetods.CreateCar);

app.MapGet("/car/{id}", CarApiMetods.GetCar);

app.MapGet("/cars", async (IQuerySession querySession) =>
{
    var result = await querySession.LoadAsync<CarSummary>(new Guid(Constants.UserId));

    return Results.Ok(result.CarBrands.Select(e => new { Id = e.Id, Brand = e.Brand, Description = e.Description, Model = e.Model}));
});

app.MapPut("/car/{id}", async ([FromBody] UpdateCarRequestModel carRequestModel, [FromRoute] string id,  IDocumentSession session) =>
{
    var guidId = new Guid(id);
    var result = session.Events.Append(guidId, new UpdateCar(guidId, carRequestModel.Description, new Guid(Constants.UserId)));

    session.SaveChanges();

    return Results.Ok(result.Id);
});

//Add brand
app.MapPost("/brand", async (IDocumentSession session, [FromBody] NameRequestModel model) => {

    var result = session.Events.StartStream(
        new AddBrand(model.Name));

    await session.SaveChangesAsync();

    return result.Id;
});

//Add new model
app.MapPost("/brand/{id}/model", async (IQuerySession querySession, IDocumentSession session, Guid id, [FromBody] NameRequestModel model) => {

    //This should be gathered from the read part
    var resutStreamExists = await querySession.Events.AggregateStreamAsync<BrandProject>(id);

    if (resutStreamExists is null)
    {
        return Results.BadRequest("Brand does not exists.");
    }

    var result = session.Events.StartStream(
        new AddModel(model.Name, id));

    await session.SaveChangesAsync();

    return Results.Ok(result.Id);
});

//Get model
app.MapGet("/model/{id}", async (IQuerySession querySession, Guid id) => {

    var resutStreamExists = await querySession.Events.AggregateStreamAsync<ModelProject>(id);

    if (resutStreamExists is null)
    {
        return Results.BadRequest("Model does not exists.");
    }

    return Results.Ok(new ModelResponseModel { 
        Id = resutStreamExists.Id.ToString(),
        Name = resutStreamExists.Name,
        BrandId = resutStreamExists.BrandId.ToString()
    });
});


//Update model
app.MapPut("/model/{id}", ModelApiMethods.UpdateModel);


//app.MapGet("/brand/{id}/models", async (IQuerySession querySession, Guid id) => {
//    var result = await querySession.Events.AggregateStreamAsync<ModelsProjector>(id);

//    var responseResult = new List<string>();

//    return Results.Ok(responseResult);
//});

app.MapGet("/brand/{id}", async (IQuerySession session, string id) => {

    var guidId = Guid.Parse(id);
    var result = await session.Events.AggregateStreamAsync<BrandProject>(guidId);

    if (result is null)
    { 
        return Results.BadRequest("Brand not found.");
    }

    return Results.Ok(new BrandResponseModel { 
        Id = id.ToString(),
        Name = result.Name,
    });
});

app.RegisterTestSecond();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider
        .GetService<RegisterMinApisDI>();

    services.RegisterTestEndpointFirst(app);
}

app.Run();

public class BrandProject {
    public Guid Id { get; set; }

    public string Name { get; set; }

    public void Apply(AddBrand brand)
    {
        this.Name = brand.Name;
    }
}

public class ModelProject
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public Guid BrandId { get; set; }

    public void Apply(AddModel model)
    {
        this.Name = model.Name;
        this.BrandId = model.BrandId;
    }

    public void Apply(UpdateModelName model)
    {
        this.Name = model.Name;
    }
}

public class CarProject
{
    public Guid Id { get; set; }

    public string BrandName { get; set; }

    public string ModelName { get; set; }

    public DateTime DateRegistered { get; set; }

    public string Description { get; set; }

    public TransmissionType TransmissionType { get; set; }

    public EngineType EngineType { get; set; }

    public void Apply(CreateCar model)
    {
        this.BrandName = model.Brand;
        this.ModelName = model.Model;
        this.DateRegistered = model.Year;
        this.Description = model.Description;
        this.TransmissionType = model.TransmissionType;
        this.EngineType = model.EngineType;
    }

    public void Apply(UpdateCar model)
    {
        this.Description = model.Description;
    }
}

public class CarSummary
{
    public Guid Id { get; set; }

    public List<CarModel> CarBrands { get; set; } = new();
}

public class CarSummaryProjector : MultiStreamProjection<CarSummary, Guid>
{
    public CarSummaryProjector()
    {
        Identity<IUser>(x => x.UserId);
    }

    public void Apply(CarSummary snapshot, CreateCar cc)
    {
        snapshot.CarBrands.Add(
            new CarModel {
                Id = cc.Id?.ToString() ?? string.Empty,
                Brand = cc.Brand,
                Description = cc.Description,
                Model = cc.Model 
                }
            );
    }

    public void Apply(CarSummary snapshot, UpdateCar cc)
    {
        //var entity = snapshot.CarBrands.FirstOrDefault(e => e.Id == cc.Id.ToString());
        //entity.Description = cc.Description;
    }

}


//public class ModelsProjector : SingleStreamProjection<ModelProject2>
//{ 
//    public void Apply(ModelProject2 modelP, AddModel model)
//    {
//        modelP.Models.Add(model.Name);
//    }

//    public void Apply(ModelProject2 modelP, UpdateModelName model)
//    {
//        modelP.Models.Add(model.Name);
//    }
//}

public class ModelProject2
{ 
    public Guid Id { get; set; }

    public List<string> Models { get; set; }
}

public interface IUser
{ 
    public Guid UserId { get; init; }
}

public record CreateCar(
    Guid? Id,
    string Brand, 
    string Model, 
    DateTime Year, 
    string Description,
    EngineType EngineType,
    TransmissionType TransmissionType,
    Guid UserId) : IUser;

public record UpdateCar (
        Guid Id,
        string Description,
        Guid UserId
    ) : IUser;

public record AddModel(string Name, Guid BrandId);

public record AddBrand(string Name);

public record UpdateModelName(string Name);

public class Brand {
    public string Name { get; set; }
};

public class Model {
    public string Name { get; set; }

    public Guid BrandId { get; set; }
}

public class NameRequestModel
{
    public string Name { get; set; }
}

public class BrandResponseModel
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public class ModelResponseModel
{
    public string Id { get; set; }
    public string Name { get; set; }

    public string BrandId { get; set; }
}

public record IdAndValueModel(string Id, string Value);


