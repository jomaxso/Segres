using System.Collections.Concurrent;
using FluentValidation;
using Segres;
using Segres.AspNet;
using Segres.Behaviors;
using Segres.Contracts;
using WeatherForecastDemo.Application;
using WeatherForecastDemo.Application.Commons.Behaviors;
using WeatherForecastDemo.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.
    builder.Services.AddInfrastructure();
    //
    // builder.Services.AddCors();

    builder.Services.AddSegres(options => options
        .AsScoped()
        .WithCustomPublisher<MyPublisher>()
        .WithParallelNotificationHandling());

    // builder.Services.AddScoped(typeof(IAsyncRequestBehavior<,>), typeof(QueryValidatorBehavior<,>));
    builder.Services.AddScoped(typeof(IAsyncRequestBehavior<,>), typeof(QueryValidatorBehavior<,>));
    builder.Services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(includeInternalTypes: true);

    builder.Services.AddAuthorization();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    
    builder.Services.AddHostedService<NotificationWorker>();
}

var app = builder.Build();
{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    
    app.UseSegres();
}

// app.UseCors(x => x.AllowAnyOrigin());
app.Run();

public class NotificationWorker : BackgroundService
{
    private readonly ISubscriber _subscriber;

    public NotificationWorker(ISubscriber subscriber)
    {
        _subscriber = subscriber;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(20000, stoppingToken);
            
            for (var i = 0; i < 20; i++)
            {
                if (MyPublisher.Notifications.TryDequeue(out var notification))
                {
                    await _subscriber.SubscribeAsync(notification, stoppingToken);
                }
            }
        }
    }
}

public sealed class MyPublisher : IPublisher
{
    public static readonly ConcurrentQueue<INotification> Notifications = new();

    public ValueTask PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default) 
        where TNotification : INotification
    {
        Notifications.Enqueue(notification);
        return ValueTask.CompletedTask;
    }
}