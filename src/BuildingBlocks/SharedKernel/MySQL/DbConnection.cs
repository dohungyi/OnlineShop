using System.Data;
using Dapper;
using MySqlConnector;
using SharedKernel.Domain;

namespace SharedKernel.MySQL;

public class DbConnection : IDbConnection
{
    #region Declare + Constructor
    private readonly MySqlConnection _connection;
    private readonly string _dbConfig;
    private MySqlTransaction _transaction;
    private ConnectionState _currentState = ConnectionState.Connecting;
    private int _numberOfConnection = 0;
    
    private List<DomainEvent> DomainEvents { get; set; } = new();

    public MySqlConnection Connection => _connection;

    public MySqlTransaction CurrentTransaction
    {
        get
        {
            if (_transaction == null || _transaction.Connection == null)
            {
                _transaction = _connection.BeginTransaction();
            }
            return _transaction;
        }
    }

    public DbConnection(string dbConfig = "DefaultDb")
    {
        _connection = new MySqlConnection(CoreSettings.CoreSettings.ConnectionStrings[dbConfig]);
        _dbConfig = dbConfig;

        if (_connection.State == ConnectionState.Closed)
        {
            _connection.Open();
            _currentState = ConnectionState.Open;
            _numberOfConnection = 1;
        }

    }
    #endregion

    public async Task CommitAsync(bool dispatchEvent = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task RollBackAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
    

    public async Task PublishEvents(IEventDispatcher eventDispatcher, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = CommandType.Text)
    {
        throw new NotImplementedException();
    }

    public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = CommandType.Text)
    {
        throw new NotImplementedException();
    }

    public async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param = null, int? commandTimeout = null,
        CommandType? commandType = CommandType.Text)
    {
        throw new NotImplementedException();
    }

    public async Task<SqlMapper.GridReader> QueryMultipleAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = CommandType.Text)
    {
        throw new NotImplementedException();
    }

    public async Task<int> ExecuteAsync(string sql, object param, int? commandTimeout = null, CommandType? commandType = CommandType.Text,
        bool autoCommit = false)
    {
        throw new NotImplementedException();
    }

    public async Task<object> ExecuteScalarAsync(string sql, object param, int? commandTimeout = null, CommandType? commandType = CommandType.Text,
        bool autoCommit = false)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<T>> ExecuteAndGetResultAsync<T>(string sql, object param, int? commandTimeout = null, CommandType? commandType = CommandType.Text,
        bool autoCommit = false)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<MySqlBulkCopyResult> WriteToServerAsync<T>(DataTable dataTable, IList<T> entites, CancellationToken cancellationToken,
        bool autoCommit = false)
    {
        throw new NotImplementedException();
    }
    
    #region Dispose
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        if (_connection != null && _connection.State == ConnectionState.Open)
        {
            _connection.Close();
        }
    }
    #endregion
}