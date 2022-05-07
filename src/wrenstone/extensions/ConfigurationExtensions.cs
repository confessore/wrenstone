using System.Text;
using wrenstone.options;

namespace wrenstone.extensions
{
    public static class ConfigurationExtensions
    {
        public static Task<string> BuildDefaultConnectionStringAsync(this IConfiguration configuration)
        {
            var options = new SqlClientOptions();
            configuration.GetSection("APPLICATION:SQLCLIENTOPTIONS").Bind(options);
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"Server={options.Server!.Trim()};");
            stringBuilder.Append($"User Id={options.Username!.Trim()};");
            stringBuilder.Append($"Password={options.Password!.Trim()};");
            stringBuilder.Append($"Database={options.Database!.Trim()}.default;");
            return Task.FromResult(stringBuilder.ToString());
        }
    }
}
