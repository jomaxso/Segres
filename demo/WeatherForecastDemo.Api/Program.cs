using Segres;
using Segres.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddSegres(options => options
        .AsScoped()
        .WithCustomPublisher<MyPublisher>()
        .WithParallelNotificationHandling());
    
    builder.Services.AddAuthorization();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
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