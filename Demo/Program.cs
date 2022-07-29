using MicrolisR;
using MicrolisR.Extensions.Microsoft.DependencyInjection;

var builder = WebApplication.CreateBuilder();
// builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Services.AddMicrolisR(typeof(Program));
builder.Services.AddEndpoints();

var app = builder.Build();

app.MapEndpoints();

app.Run();