using Microsoft.Extensions.Configuration;
using IConfiguration = Castle.Core.Configuration.IConfiguration;

namespace SharedKernel.Libraries;

public static class JsonHelper
{
    public static object _lockObj = new object();
    public static Dictionary<string, IConfigurationRoot> Pairs = new Dictionary<string, IConfigurationRoot>();
    
    public static IConfigurationRoot GetConfiguration(string jsonFileName = "appsettings.json")
    {
        //  kiểm tra xem _lockObj có đang được sử dụng bởi luồng khác hay không.
        // Nếu _lockObj đang bị "khoá" (locked) bởi luồng khác, luồng hiện tại sẽ phải chờ cho đến khi _lockObj được mở ra (unlocked)
        // trước khi nó có thể thực hiện phần mã bên trong lock.
        lock (_lockObj)
        {
            if (!Pairs.ContainsKey(jsonFileName))
            {
                Pairs.Add(jsonFileName, new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(jsonFileName, optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build()
                );
            }
        }

        return Pairs[jsonFileName];
    }
}