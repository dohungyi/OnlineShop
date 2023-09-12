using System.Data;
using Dapper;
using MySqlConnector;
using SharedKernel.Domain;
using SharedKernel.Core;
using SharedKernel.Log;
using static Dapper.SqlMapper;

namespace SharedKernel.MySQL;

public class DbConnection : IDbConnection
{
    #region [DECLARE + CONSTRUCTOR]
    
    private readonly MySqlConnection _connection;
    private readonly string _dbConfig;
    private MySqlTransaction _transaction;
    private ConnectionState _currentState = ConnectionState.Connecting;
    private int _numberOfConnection = 0;
    
    public DbConnection(string dbConfig = "DefaultDb")
    {
        _connection = new MySqlConnection(CoreSettings.ConnectionStrings[dbConfig]);
        _dbConfig = dbConfig;

        if (_connection.State == ConnectionState.Closed)
        {
            _connection.Open();
            _currentState = ConnectionState.Open;
            _numberOfConnection = 1;
        }

    }
    
    #endregion [DECLARE + CONSTRUCTOR]

    #region [IMPLEMENTATION PROPERTIES]
    
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
    
    #endregion [IMPLEMENTATION PROPERTIES]

    #region [EVENTS]

    public async Task PublishEvents(IEventDispatcher eventDispatcher, CancellationToken cancellationToken)
    {
        await Task.Yield();
        if (DomainEvents != null && DomainEvents.Any())
        {
            var events = DomainEvents.Select(x => x).ToList();
            _ = eventDispatcher.FireEvent(events, cancellationToken);

            DomainEvents.Clear();
        }
    }

    #endregion [EVENTS]

    #region [QUERIES]
    
    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = CommandType.Text)
    {
        try
        {
            if (_currentState == ConnectionState.Fetching || _currentState == ConnectionState.Executing)
            {
                using var newConnection = new MySqlConnection(CoreSettings.ConnectionStrings[_dbConfig]);
                
                try
                {
                    _numberOfConnection++;
                    return await newConnection.QueryAsync<T>(sql, param, commandTimeout: commandTimeout, commandType: commandType);
                }
                finally
                {
                    _numberOfConnection--;
                }

            }
            _currentState = ConnectionState.Fetching;
            return await _connection.QueryAsync<T>(sql, param, commandTimeout: commandTimeout, commandType: commandType);
        }
        finally
        {
            if (_numberOfConnection <= 1)
            {
                _currentState = ConnectionState.Open;
            }
        }
    }

    public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = CommandType.Text)
    {
        try
        {
            if (_currentState == ConnectionState.Fetching || _currentState == ConnectionState.Executing)
            {
                using var newConnection = new MySqlConnection(CoreSettings.ConnectionStrings[_dbConfig]);
                
                try
                {
                    _numberOfConnection++;
                    return await newConnection.QueryFirstOrDefaultAsync<T>(sql, param, commandTimeout: commandTimeout, commandType: commandType);
                }
                finally
                {
                    _numberOfConnection--;
                }
                
            }
            
            _currentState = ConnectionState.Fetching;
            return await _connection.QueryFirstOrDefaultAsync<T>(sql, param, commandTimeout: commandTimeout, commandType: commandType);
        }
        finally
        {
            if (_numberOfConnection <= 1)
            {
                _currentState = ConnectionState.Open;
            }
        }
    }

    public async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param = null, int? commandTimeout = null,
        CommandType? commandType = CommandType.Text)
    {
        try
        {
            if (_currentState == ConnectionState.Fetching || _currentState == ConnectionState.Executing)
            {
                using var newConnection = new MySqlConnection(CoreSettings.ConnectionStrings[_dbConfig]);
                
                try
                {
                    _numberOfConnection++;
                    return await newConnection.QuerySingleOrDefaultAsync<T>(sql, param, commandTimeout: commandTimeout, commandType: commandType);
                }
                finally
                {
                    _numberOfConnection--;
                }
                
            }
            
            _currentState = ConnectionState.Fetching;
            return await _connection.QuerySingleOrDefaultAsync<T>(sql, param, commandTimeout: commandTimeout, commandType: commandType);
        }
        finally
        {
            if (_numberOfConnection <= 1)
            {
                _currentState = ConnectionState.Open;
            }
        }
    }

    public async Task<GridReader> QueryMultipleAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = CommandType.Text)
    {
        try
        {
            if (_currentState == ConnectionState.Fetching || _currentState == ConnectionState.Executing)
            {
                using (var newConnection = new MySqlConnection(CoreSettings.ConnectionStrings[_dbConfig]))
                {
                    try
                    {
                        _numberOfConnection++;
                        return await newConnection.QueryMultipleAsync(sql, param, CurrentTransaction, commandTimeout: commandTimeout, commandType: commandType);
                    }
                    finally
                    {
                        _numberOfConnection--;
                    }
                }
            }

            _currentState = ConnectionState.Fetching;
            return await _connection.QueryMultipleAsync(sql, param, CurrentTransaction, commandTimeout: commandTimeout, commandType: commandType);
        }
        finally
        {
            if (_numberOfConnection <= 1)
            {
                _currentState = ConnectionState.Open;
            }
        }
    }
    
    #endregion [QUERIES]

    #region [COMMANDS]
    
    public async Task<int> ExecuteAsync(string sql, object param, int? commandTimeout = null, CommandType? commandType = CommandType.Text,
        bool autoCommit = false)
    {
        try
        {
            _currentState = ConnectionState.Executing;
            if (param is not null && (param is IBaseEntity @base))
            {
                if (@base.DomainEvents is not null && @base.DomainEvents.Any())
                {
                    DomainEvents.AddRange(@base.DomainEvents);
                }
            }

            var result = await _connection.ExecuteAsync(sql, param, CurrentTransaction, commandTimeout, commandType);
            if (autoCommit)
            {
                await CommitAsync();
            }

            return result;
        }
        finally
        {
            if (_numberOfConnection <= 1)
            {
                _currentState = ConnectionState.Open;
            }
        }
    }

    public async Task<object> ExecuteScalarAsync(string sql, object param, int? commandTimeout = null, CommandType? commandType = CommandType.Text,
        bool autoCommit = false)
    {
        try
        {
            _currentState = ConnectionState.Executing;
            if (param is not null && param is IBaseEntity @base)
            {
                if (@base.DomainEvents is not null && @base.DomainEvents.Any())
                {
                    DomainEvents.AddRange(@base.DomainEvents);
                }
            }

            var result = await _connection.ExecuteScalarAsync(sql, param, CurrentTransaction, commandTimeout, commandType);
            if (autoCommit)
            {
                await CommitAsync();
            }

            return result;
        }
        finally
        {
            if (_numberOfConnection <= 1)
            {
                _currentState = ConnectionState.Open;
            }
        }
    }

    public async Task<IEnumerable<T>> ExecuteAndGetResultAsync<T>(string sql, object param, int? commandTimeout = null, CommandType? commandType = CommandType.Text, bool autoCommit = false)
        {
            try
            {
                _currentState = ConnectionState.Executing;
                if (param is not null && param is IBaseEntity @base)
                {
                    if (@base.DomainEvents is not null && @base.DomainEvents.Any())
                    {
                        DomainEvents.AddRange(@base.DomainEvents);
                    }
                }
                
                return await _connection.QueryAsync<T>(sql, param, CurrentTransaction, commandTimeout: commandTimeout, commandType: commandType);
            }
            finally
            {
                if (_numberOfConnection <= 1)
                {
                    _currentState = ConnectionState.Open;
                }
            }
        }

        public async ValueTask<MySqlBulkCopyResult> WriteToServerAsync<T>(DataTable dataTable, IList<T> entites, CancellationToken cancellationToken, bool autoCommit = false)
        {
            try
            {
                _currentState = ConnectionState.Executing;
                if (typeof(IBaseEntity).IsAssignableFrom(typeof(T)))
                {
                    foreach (var entity in entites)
                    {
                        var @base = (IBaseEntity)entity;
                        if (@base.DomainEvents != null && @base.DomainEvents.Any())
                        {
                            DomainEvents.AddRange(@base.DomainEvents);
                        }
                    }
                }

                // Create object of MySqlBulkCopy which help to insert  
                var bulk = new MySqlBulkCopy(_connection, CurrentTransaction);

                // Assign(Gán) Destination table name  
                bulk.DestinationTableName = dataTable.TableName;

                // Mapping column
                int index = 0;
                foreach (DataColumn col in dataTable.Columns)
                {
                    bulk.ColumnMappings.Add(new MySqlBulkCopyColumnMapping(index++, col.ColumnName));
                }

                await _connection.ExecuteAsync("SET GLOBAL local_infile=1", null, CurrentTransaction);

                // Insert bulk Records into DataBase.
                _connection.InfoMessage += (s, e) =>
                {
                    Logging.Error(string.Join(" ----> ", e.Errors.Select(e => e.Message)));
                };

                var result = await bulk.WriteToServerAsync(dataTable, cancellationToken);
                if (autoCommit)
                {
                    await CommitAsync(cancellationToken: cancellationToken);
                }

                return result;
            }
            finally
            {
                if (_numberOfConnection <= 1)
                {
                    _currentState = ConnectionState.Open;
                }
            }
        }
    
    #endregion [COMMANDS]

    #region [UNIT OF WORK]

    public async Task CommitAsync(bool dispatchEvent = true, CancellationToken cancellationToken = default)
    {
        await CurrentTransaction.CommitAsync(cancellationToken);
        if (!dispatchEvent)
        {
            DomainEvents.Clear();
        }
    }

    public async Task RollBackAsync(CancellationToken cancellationToken = default)
    {
        await CurrentTransaction.RollbackAsync(cancellationToken);
    }

    #endregion [UNIT OF WORK]

    #region [DISPOSE]
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        if (_connection != null && _connection.State == ConnectionState.Open)
        {
            _connection.Close();
        }
    }
    
    #endregion [DISPOSE]
    
}