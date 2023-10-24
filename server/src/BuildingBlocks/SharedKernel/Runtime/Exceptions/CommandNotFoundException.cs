using System.Runtime.Serialization;

namespace SharedKernel.Runtime.Exceptions;

public class CommandNotFoundException : Exception
{
    public CommandNotFoundException() : this(string.Empty)
    {
    }

    public CommandNotFoundException(string message) : base(message)
    {
    }

    public CommandNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected CommandNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}