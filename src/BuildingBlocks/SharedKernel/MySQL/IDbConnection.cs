using Dapper;
using System.Data;
using MySqlConnector;
using SharedKernel.Domain;
using SharedKernel.UnitOfWork;

namespace SharedKernel.MySQL;

public interface IDbConnection : IUnitOfWork
{
    MySqlConnection Connection { get; }

    MySqlTransaction CurrentTransaction { get; }

    Task PublishEvents(IEventDispatcher eventDispatcher, CancellationToken cancellationToken);

    /// <summary>
    ///  Execute a query asynchronously using Task.
    /// </summary>
    Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = CommandType.Text);

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = CommandType.Text);

    /// <summary>
    ///  Execute a single-row query asynchronously using Task.
    /// </summary>
    Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = CommandType.Text);

    /// <summary>
    /// Execute a command that returns multiple result sets, and access each in turn.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    Task<SqlMapper.GridReader> QueryMultipleAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = CommandType.Text);

    /// <summary>
    /// Execute a command asynchronously using Task.
    /// </summary>
    /// <returns> The number of rows affected. </returns>
    Task<int> ExecuteAsync(string sql, object param, int? commandTimeout = null, CommandType? commandType = CommandType.Text, bool autoCommit = false);

    /// <summary>
    /// Execute parameterized SQL that selects a single value.
    /// </summary>
    /// <returns> The first cell selected as System.Object. </returns>
    Task<object> ExecuteScalarAsync(string sql, object param, int? commandTimeout = null, CommandType? commandType = CommandType.Text, bool autoCommit = false);

    Task<IEnumerable<T>> ExecuteAndGetResultAsync<T>(string sql, object param, int? commandTimeout = null, CommandType? commandType = CommandType.Text, bool autoCommit = false);

    /// <summary>
    /// Asynchronously copies all rows in the supplied <see cref="DataTable"/> to the destination table specified by the
    /// <see cref="DestinationTableName"/> property of the <see cref="MySqlBulkCopy"/> object.
    /// </summary>
    /// <param name="dataTable">The <see cref="DataTable"/> to copy.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="MySqlBulkCopyResult"/> with the result of the bulk copy operation.</returns>
    ValueTask<MySqlBulkCopyResult> WriteToServerAsync<T>(DataTable dataTable, IList<T> entites, CancellationToken cancellationToken, bool autoCommit = false);
}