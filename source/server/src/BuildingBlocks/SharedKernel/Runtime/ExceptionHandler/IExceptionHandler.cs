namespace SharedKernel.Persistence.ExceptionHandler;

public interface IExceptionHandler
{
    Task PutToDatabaseAsync(Exception ex);
}