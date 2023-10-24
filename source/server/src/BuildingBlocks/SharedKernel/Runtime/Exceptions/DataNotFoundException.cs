using System.Runtime.Serialization;

namespace SharedKernel.Runtime.Exceptions;

public class DataNotFoundException : Exception
{
    public DataNotFoundException() : this(string.Empty)
    {
    }

    public DataNotFoundException(string message) : base(message)
    {
    }

    public DataNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
    
    protected DataNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}