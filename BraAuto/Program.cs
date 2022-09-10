using BraAuto.AspNetCore.Authentication.Cookies;
using BraAuto.Helpers.Log;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using NLog.Web;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

logger.Debug("Starting app");
try
{
    var builder = WebApplication.CreateBuilder(args);


    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog();

    //db connections
    BraAutoDb.Dal.Db.SetupConnection(builder.Configuration.GetConnectionString("BraAutoDb"));

    // Add services to the container.
    builder.Services
        .AddControllersWithViews()
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        });

    builder.Services
       .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
       .AddCookie(options =>
       {
           options.LoginPath = new PathString("/Users/Login");
           options.AccessDeniedPath = new PathString("/Errors/NoAccess");
           options.Cookie.Name = "bra_auto_auth";
           options.EventsType = typeof(BraAutoCookieAuthenticationEvents);
       });
    builder.Services.AddScoped<BraAutoCookieAuthenticationEvents>();
    builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
    builder.Services.AddSingleton<ILogHelper, LogHelper>();
    builder.Services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();
    //builder.Services.AddSingleton<IEmailSender, EmailSender>();
    builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
           name: "default",
           pattern: "{controller=Cars}/{action=Home}/{id?}");

    //app.MapRazorPages();

    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}
