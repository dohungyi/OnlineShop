using Microsoft.EntityFrameworkCore;
using SharedKernel.Application;

namespace SharedKernel.Persistence;

public interface IAppDbContext<TDbContext> : IUnitOfWork<TDbContext> where TDbContext : DbContext
{
    
}