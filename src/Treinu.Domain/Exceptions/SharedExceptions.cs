namespace Treinu.Domain.Exceptions;

public class InvalidPropsException : Exception
{
    public InvalidPropsException(string message) : base(message)
    {
    }
}

public class RepositoryException : Exception
{
    public RepositoryException(string message) : base(message)
    {
    }
}

public class UseCaseException : Exception
{
    public UseCaseException(string message) : base(message)
    {
    }
}