using Npgsql.PostgresTypes;
using SharedKernel.Libraries;

namespace SharedKernel.Application;

public class Filter
{
    public string SearchString { get; set; }
    public List<Field> Fields { get; set; }
}