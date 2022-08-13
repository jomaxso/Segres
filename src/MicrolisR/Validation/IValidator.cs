namespace MicrolisR.Validation;

/// <summary>
/// Defines a mediator to encapsulate request/response and publisher/subscriber patterns as well as providing several common functionalities related to these patterns.
/// </summary>
/// <seealso cref="IValidatable"/>
/// <seealso cref="IValidation{T}"/>
public interface IValidator
{
    
    /// <summary>
    /// Validate an object throw any defined validation handler of the same type. 
    /// </summary>
    /// <param name="value">The object to validate.</param>
    /// <typeparam name="T">The object type.</typeparam>
    /// <seealso cref="IValidatable"/>
    /// <seealso cref="IValidation{T}"/>
    public void Validate<T>(T value)
        where T : IValidatable;
}