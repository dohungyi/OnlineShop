using System.Runtime.Serialization;

namespace SharedKernel.Runtime.Exceptions;

public class SqlInjectionException : Exception
{
    public SqlInjectionException() : this(string.Empty)
    {
    }

    public SqlInjectionException(string message) : base(message)
    {
    }

    public SqlInjectionException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected SqlInjectionException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}