namespace MicrolisR.Mapping.Exceptions;

internal class DuplicateMapperRegistrationException : Exception
{
    public DuplicateMapperRegistrationException(string message) : base(message)
    {

    }
}