using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace SharedKernel.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    
}