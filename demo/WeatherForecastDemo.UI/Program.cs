// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Segres;
using WeatherForecastDemo.Contracts.WeatherForecast;

IServiceProvider Provider = new ServiceCollection()
    .AddSegres()
    .AddScoped(typeof(IRequestBehavior<,>), typeof(Validator<,>))
    .AddTransient<IRequestBehavior<ThePerson, int>, ThePersonHandlerValidatorTwo>()
    .AddTransient<IRequestBehavior<ThePerson, int>, ThePersonHandlerValidatorThree>()
    .BuildServiceProvider();


var result = await Provider.GetService<ISender>()!.SendAsync(new ThePerson(0), CancellationToken.None);
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