namespace SharedKernel.Persistence.ExceptionHandler;

public class ExceptionHandler : IExceptionHandler
{
    public async Task PutToDatabaseAsync(Exception exception)
    {
        // using (var dbConnection = new DbConnection())
        // {
        //     var cmd = $"INSERT INTO {new ExceptionModel().GetTableName()}(Source, Message, StackTrace) VALUES (@Source, @Message, @StackTrace)";
        //     await dbConnection.ExecuteAsync(cmd, new ExceptionModel(exception.Source, exception.Message, exception.StackTrace), autoCommit: true);
        // }
    }
}