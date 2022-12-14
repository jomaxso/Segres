﻿// See https://aka.ms/new-console-template for more information

using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Segres;

IServiceProvider Provider = new ServiceCollection()
    .AddSegres(configuration => configuration
        .WithHttpClient<ThePerson>(""))

    .AddValidatorsFromAssembly(Assembly.GetEntryAssembly())
    .BuildServiceProvider();


Result<int> result = await Provider
    .GetService<ISender>()!
    .SendAsync(new ThePerson(200), CancellationToken.None);

if (result.IsSuccess)
{
    Console.WriteLine("jUP");
}


Console.WriteLine();


return;

// var serviceContainer = new ServiceCollection();
// {
//     serviceContainer.AddHttpClient("HUHU", x => x.BaseAddress = new Uri("https://localhost:7080/"));
//     serviceContainer.AddSegres(x => x.RegisterAssembly(typeof(CreateWeatherForecastRequest).Assembly));
// }
//
// var serviceResolver = serviceContainer.BuildServiceProvider();
//ISender sender = serviceResolver.GetRequiredService<ISender>();

// var request = new GetAllRequestTest();
//
// var enumerable = await sender.SendAsync(request);
//
// foreach (var i in enumerable!)
// {
//     await Task.Delay(100);
//     Console.WriteLine(i);
// }