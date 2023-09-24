using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace SharedKernel.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    
    public Task CommitAsync(bool dispatchEvent = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task RollBackAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
    
}