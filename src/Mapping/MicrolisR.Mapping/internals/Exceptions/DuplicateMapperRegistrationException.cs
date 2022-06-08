namespace MicrolisR.Mapping.internals.Exceptions;

internal class DuplicateMapperRegistrationException : Exception
{
    public DuplicateMapperRegistrationException(string message) : base(message)
    {

    }
}