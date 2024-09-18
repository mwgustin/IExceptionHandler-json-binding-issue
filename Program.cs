using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<DefaultExceptionHandler>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseExceptionHandler(); //current .net bug requires a default builder.  

app.MapPost("/test", (Test testmsg)=>
{
    return Results.Ok($"Message received: {testmsg.Message}");
})
.WithName("PostTest")
.WithOpenApi();

app.MapGet("/bad", () =>
{
    throw new Exception("This is a test exception");
})
.WithName("ExceptionTest")
.WithOpenApi();



app.Run();

record Test
{
    public string Message {get;set;}
}

public class DefaultExceptionHandler : IExceptionHandler
{
    private readonly ILogger<DefaultExceptionHandler> _logger;

    public DefaultExceptionHandler(ILogger<DefaultExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, exception.Message);
        await Results.Ok($"Something Went Wrong. {exception.Message}").ExecuteAsync(context); //force an OK response with error details. 
        return true;
    }
}