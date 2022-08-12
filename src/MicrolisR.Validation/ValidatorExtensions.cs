using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace MicrolisR.Validation;

public static class ValidationExtensions
{
    public static ValidationContext<T?> RuleFor<T>(this T? toValidate, [CallerArgumentExpression("toValidate")] string paramName = "")
        where T : class
    {
        return new ValidationContext<T?>(toValidate, paramName);
    }

    public static ValidationContext<U> RuleFor<T, U>(this T toValidate, Func<T, U> onValidate, [CallerArgumentExpression("toValidate")] string paramName = "")
    {
        return new ValidationContext<U>(onValidate(toValidate), paramName);
    }

    # region Nummerics

    public static ValidationContext<int> IsGreaterThen(this ValidationContext<int> numericContext, int value)
    {
        if (numericContext.Value > value)
            return numericContext;

        throw new Exception($"{numericContext.FieldName} is less then {value}");
    }

    public static ValidationContext<int> IsGreaterOrEqualThen(this ValidationContext<int> numericContext, int value)
    {
        if (numericContext.Value >= value)
            return numericContext;

        throw new Exception($"{numericContext.FieldName} is less then {value}");
    }

    public static ValidationContext<int> IsLessThen(this ValidationContext<int> numericContext, int value)
    {
        if (numericContext.Value < value)
            return numericContext;

        throw new Exception($"{numericContext.FieldName} is greater then {value}");
    }

    public static ValidationContext<int> IsLessOrEqualThen(this ValidationContext<int> numericContext, int value)
    {
        if (numericContext.Value <= value)
            return numericContext;

        throw new Exception($"{numericContext.FieldName} is greater then {value}");
    }

    public static ValidationContext<int> IsEqualTo(this ValidationContext<int> numericContext, int value)
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

    public static ValidationContext<int> IsExclusiveBetween(this ValidationContext<int> numericContext, int lower, int upper)
    {
        if (numericContext.Value <= lower || numericContext.Value >= upper)
            throw new Exception($"{numericContext.FieldName} is not between {lower} and {upper}");

        return numericContext;
    }

    public static ValidationContext<int> IsBetween(this ValidationContext<int> numericContext, int lower, int upper)
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

    public static ValidationContext<T?> IsNull<T>(this ValidationContext<T?> numericContext)
        where T : class
    {
        if (numericContext.Value is null)
            return numericContext;

        throw new Exception($"{numericContext.FieldName} is not null");
    }

    public static ValidationContext<T> IsNotNull<T>(this ValidationContext<T?> numericContext)
        where T : class
    {
        if (numericContext.Value is not null)
            return numericContext!;

        throw new Exception($"{numericContext.FieldName} is null");
    }

    # endregion
}