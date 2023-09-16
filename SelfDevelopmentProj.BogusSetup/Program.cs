using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelfDevelopmentProj.BogusSetup;
using SelfDevelopmentProj.BogusSetup.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BloggingContext>(options => {
    options.UseSqlite("Data Source=blogging.db");
    options.EnableSensitiveDataLogging();
});
// Add services to the container.
builder.Services.AddSingleton<IAccumulatorQueue, AccumulatorQueue>();
builder.Services.AddHostedService<AccumulatorBackgroundService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapGet("/setData", ([FromServices] BloggingContext ctx) =>
{
    try
    {
        FakeDataA.Init(10);

        ctx.Blogs.AddRange(FakeDataA.blogs);
        ctx.Posts.AddRange(FakeDataA.posts);

        ctx.SaveChanges();
        //var blogs = ctx.Blogs
        //   .Include(b => b.Posts)
        //   .ToList();

        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest();
    }
   
});

app.MapGet("/getdata", ([FromServices] BloggingContext ctx) =>
{
    //var blogs = ctx.Blogs.Count();

    var blogs = ctx.Blogs
       .Include(b => b.Posts)
       .ToList();

    return Results.Ok(blogs.Select(e => new { blogId = e.BlogId, url = e.Url, posts = e.Posts.Select(p => new { postId = p.PostId, content = p.Content, title = p.Title}).ToList()}));
});

app.MapGet("/sentshortmessage1/{message}", async ([FromServices] IAccumulatorQueue accQueue, string message) =>
{
    Console.WriteLine("Do some stuff");

    await accQueue.PushAsync(message);
});

app.MapGet("/sentshortmessage2/{message}", async ([FromServices] IAccumulatorQueue accQueue, string message) =>
{

    Console.WriteLine("Do some stuff");

    await accQueue.PushAsync(message);
});

app.Run();
