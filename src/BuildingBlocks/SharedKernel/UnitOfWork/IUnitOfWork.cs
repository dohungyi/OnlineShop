namespace SharedKernel.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    Task CommitAsync(bool dispatchEvent = true, CancellationToken cancellationToken = default);

    Task RollBackAsync(CancellationToken cancellationToken = default);
}