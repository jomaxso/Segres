using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace MicrolisR.Validation;

public static class ValidationExtensions
{
    public static IValidationContext<T?> RuleFor<T>(this T? toValidate, [CallerArgumentExpression("toValidate")] string paramName = "")
        where T : class
    {
        return new ValidationContext<T?>(toValidate, ref paramName);
    }

    public static IValidationContext<U> RuleFor<T, U>(this T toValidate, Func<T, U> onValidate, [CallerArgumentExpression("toValidate")] string paramName = "")
    {
        return new ValidationContext<U>(onValidate(toValidate), ref paramName);
    }

    # region Nummerics

    public static IValidationContext<int> IsGreaterThen(this IValidationContext<int> numericContext, int value)
    {
        if (numericContext.Value > value)
            return numericContext;

        throw new Exception($"{numericContext.FieldName} is less then {value}");
    }

    public static IValidationContext<int> IsGreaterOrEqualThen(this IValidationContext<int> numericContext, int value)
    {
        if (numericContext.Value >= value)
            return numericContext;

        throw new Exception($"{numericContext.FieldName} is less then {value}");
    }

    public static IValidationContext<int> IsLessThen(this IValidationContext<int> numericContext, int value)
    {
        if (numericContext.Value < value)
            return numericContext;

        throw new Exception($"{numericContext.FieldName} is greater then {value}");
    }

    public static IValidationContext<int> IsLessOrEqualThen(this IValidationContext<int> numericContext, int value)
    {
        if (numericContext.Value <= value)
            return numericContext;

        throw new Exception($"{numericContext.FieldName} is greater then {value}");
    }

    public static IValidationContext<int> IsEqualTo(this IValidationContext<int> numericContext, int value)
    {
        if (numericContext.Value == value)
            return numericContext;

        throw new Exception($"{numericContext.FieldName} is not equal to {value}");
    }

    // public static ValidationContext<int> IsAlmostEqualTo(this ValidationContext<int> numericContext, int value, int tolerance)
    // {
    //     if (numericContext.Value == value)
    //         return numericContext;
    //
    //     if (numericContext.Value < numericContext.Value + tolerance && numericContext.Value > numericContext.Value - tolerance)
    //         return numericContext;
    //
    //     throw new Exception($"{numericContext.Value} is not equal to {value}");
    // }

    public static IValidationContext<int> IsExclusiveBetween(this IValidationContext<int> numericContext, int lower, int upper)
    {
        if (numericContext.Value <= lower || numericContext.Value >= upper)
            throw new Exception($"{numericContext.FieldName} is not between {lower} and {upper}");

        return numericContext;
    }

    public static IValidationContext<int> IsBetween(this IValidationContext<int> numericContext, int lower, int upper)
    {
        if (numericContext.Value < lower || numericContext.Value > upper)
            throw new Exception($"{numericContext.FieldName} is not between {lower - 1} and {upper + 1}");

        return numericContext;
    }

    # endregion

    #region Strings

    #endregion

    #region Enumerables

    #endregion

    # region Objects

    public static IValidationContext<T?> IsNull<T>(this IValidationContext<T?> numericContext)
        where T : class
    {
        if (numericContext.Value is null)
            return numericContext;

        throw new Exception($"{numericContext.FieldName} is not null");
    }

    public static IValidationContext<T> IsNotNull<T>(this IValidationContext<T?> numericContext)
        where T : class
    {
        if (numericContext.Value is not null)
            return numericContext!;

        throw new Exception($"{numericContext.FieldName} is null");
    }

    # endregion
}