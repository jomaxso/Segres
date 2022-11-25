using FluentValidation;
using WeatherForecastDemo.Application.WeatherForecast.Commands;

namespace WeatherForecastDemo.Application.WeatherForecast.Queries;

internal sealed class GetAllWeatherForecastQueryValidator : AbstractValidator<GetAllWeatherForecastQuery>
{
    public GetAllWeatherForecastQueryValidator()
    {
        RuleFor(x => x.Number).NotNull();
    }
}

internal sealed class CreateWeatherForecastCommandValidator : AbstractValidator<CreateWeatherForecastCommand>
{
    public CreateWeatherForecastCommandValidator()
    {
        RuleFor(x => x.Summary).NotNull().NotEqual("string");
    }
}