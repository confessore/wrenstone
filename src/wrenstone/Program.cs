using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Serilog;
using Serilog.Events;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using wrenstone.contexts;
using wrenstone.extensions;
using wrenstone.options;
using wrenstone.services;
using wrenstone.statics;

var loggerConfig = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Logger(config =>
    {
        config
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File(Logs.CallingAssembly);
    });
Log.Logger = loggerConfig.CreateLogger();
Log.Information("updating environment variables...");
foreach (DictionaryEntry environmentVariable in Environment.GetEnvironmentVariables())
{
    if (environmentVariable.Key.ToString()!.Contains("APPLICATION") && environmentVariable.Value!.ToString()!.StartsWith('/'))
        Environment.SetEnvironmentVariable(environmentVariable.Key.ToString()!, await File.ReadAllTextAsync(environmentVariable.Value.ToString()!));
}
Log.Information("environment variables updated!");
Log.Information("building configuration...");
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
    .AddEnvironmentVariables()
    .Build();
Log.Information("configuration built!");

var defaultConnection = await configuration.BuildDefaultConnectionStringAsync();
var discordOptions = new DiscordOptions();
configuration.GetSection("APPLICATION:DISCORDOPTIONS").Bind(discordOptions);
var options = new WebApplicationOptions()
{
    ApplicationName = Strings.ExecutingAssemblyName,
    Args = args,
    WebRootPath = "wwwroot"
};
var builder = WebApplication.CreateBuilder(options);
builder.Host
    .UseSerilog();
builder.WebHost
    .ConfigureAppConfiguration(x =>
    {
        x.AddConfiguration(configuration);
    })
    .ConfigureServices(async x =>
    {
        x.AddDbContextPool<DefaultDbContext>(x =>
        {
            x.UseMySql(defaultConnection, ServerVersion.AutoDetect(defaultConnection), x =>
            {
                x.MigrationsAssembly(Strings.ExecutingAssemblyName);
            });
            x.EnableSensitiveDataLogging();
            x.EnableDetailedErrors();
        });
        x.AddDbContextFactory<DefaultDbContext>(x =>
        {
            x.UseMySql(defaultConnection, ServerVersion.AutoDetect(defaultConnection), x =>
            {
                x.MigrationsAssembly(Strings.ExecutingAssemblyName);
            });
            x.EnableSensitiveDataLogging();
            x.EnableDetailedErrors();
        });
        //x.AddIdentity<User, Role>(x =>
        //{
        //    x.User.RequireUniqueEmail = true;
        //})
        //.AddEntityFrameworkStores<DefaultDbContext>()
        //.AddDefaultTokenProviders();
        x.ConfigureSameSiteNone();
        //x.AddAuthentication()
        //    .AddCookie();
        x.AddRazorPages(x =>
        {
            x.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
        });
        x.AddServerSideBlazor();
        x.AddControllers();
        x.AddHttpContextAccessor();
        x.AddHttpClient(
            Assembly.GetCallingAssembly().GetName().Name,
            x =>
            {
                //x.BaseAddress = new Uri("http://armory.twinstar.cz/character-sheet.xml?r=KronosIV&cn=");
                x.DefaultRequestHeaders.Add("User-Agent", "wrenstone");
            });
        x.AddScoped<RoleService>();
        x.AddScoped<ArmoryService>();
        //x.AddHttpClient(Strings.CallingAssemblyName, client => client.BaseAddress = new Uri("http://localhost:5000"));
        //x.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient(Strings.CallingAssemblyName));
        //x.AddScoped<IAuthService, AuthService>();
        //x.AddScoped<ILocalStorageService, LocalStorageService>();
        //x.AddScoped<IMarketService, MarketService>();
        //x.AddScoped<IAtomService, AtomService>();
        //x.AddScoped<IUserService, UserService>();
        //x.AddScoped<AuthenticationStateProvider, DefaultRevalidatingServerAuthenticationStateProvider>();
        //x.AddScoped<IHostEnvironmentAuthenticationStateProvider>(x =>
        //{
        //    // this is safe because
        //    //     the `RevalidatingServerAuthenticationStateProvider` extends the `ServerAuthenticationStateProvider`
        //    var provider = (ServerAuthenticationStateProvider)x.GetRequiredService<AuthenticationStateProvider>();
        //    return provider;
        //});
        //x.AddScoped<CircuitHandler, DefaultCircuitHandler>();
        x.AddDiscordSocketClient();
        //await x.AddQuartzAsync();
    });
try
{
    var webApplication = builder.Build();

    await webApplication.MigrateDefaultDbContextAsync();

    await webApplication.InitializeDiscordSocketClientAsync(discordOptions);

    // Configure the HTTP request pipeline.
    if (!webApplication.Environment.IsDevelopment())
    {
        webApplication.UseExceptionHandler("/Error");
    }
    webApplication.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });
    webApplication.UseCookiePolicy(new CookiePolicyOptions
    {
        MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Lax
    });
    webApplication.UseStaticFiles();
    webApplication.UseRouting();
    webApplication.UseAuthentication();
    webApplication.UseAuthorization();
    webApplication.UseEndpoints(x =>
    {
        x.MapControllers();
        x.MapBlazorHub();
        x.MapFallbackToPage("/_Host");
    });
    await webApplication.RunAsync("http://*:5000");
}
catch (Exception e)
{
    if (e.GetType().Name.Equals("StopTheHostException", StringComparison.Ordinal))
        throw;

    Log.Fatal(e, "host terminated unexpectedly...");
    if (Debugger.IsAttached && Environment.UserInteractive)
    {
        Console.WriteLine(string.Concat(Environment.NewLine, "press any key to exit..."));
        Console.Read();
    }
}
finally
{
    Log.CloseAndFlush();
}
