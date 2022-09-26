using MicrolisR;
using MicrolisR.Extensions.Microsoft.DependencyInjection;
using WebApplication1.Endpoints;
using WebApplication1.Endpoints.WeatherForecast;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddMicrolisR(typeof(Program));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<ValidationBehavior>();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapGet("/", (IDispatcher mediator, CancellationToken cancellationToken) => mediator.SendAsync(new GetAll(), cancellationToken));
app.MapGet("/{id:int}", (IDispatcher mediator, int id, CancellationToken cancellationToken) => mediator.SendAsync(new GetById(){Id = id}, cancellationToken));
app.MapPost("/", (IDispatcher mediator, CreateRequest request, CancellationToken cancellationToken) => mediator.SendAsync(request, cancellationToken));
app.MapPut("/{id:int}", (IDispatcher mediator, int id, Update request, CancellationToken cancellationToken) =>
{
    request.Id = id;
    return mediator.SendAsync(request, cancellationToken);
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.Run();