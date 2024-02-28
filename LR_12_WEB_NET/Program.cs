using System.Net;
using System.Text;
using System.Text.Json;
using LR_12_WEB_NET.ApiClient;
using LR_12_WEB_NET.Data.DatabaseContext;
using LR_12_WEB_NET.Enums;
using LR_12_WEB_NET.Hubs;
using LR_12_WEB_NET.Jobs;
using LR_12_WEB_NET.Models.Config;
using LR_12_WEB_NET.QuartzJobs;
using LR_12_WEB_NET.Services;
using LR_12_WEB_NET.Services.AuthService;
using LR_12_WEB_NET.Services.BackgroundEmailNotificationTaskQueue;
using LR_12_WEB_NET.Services.ListingsMemoryCache;
using LR_12_WEB_NET.Services.ListingsMemoryCacheService;
using LR_12_WEB_NET.Services.QuoteService;
using LR_12_WEB_NET.Services.UserRoleService;
using LR_12_WEB_NET.Services.UserService;
using LR6_WEB_NET.Extensions;
using LR6_WEB_NET.Models.Database;
using LR6_WEB_NET.Models.Enums;
using MailKit.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Display;
using Serilog.Sinks.Email;


SmtpConfig? smtpConfig = JsonSerializer.Deserialize<SmtpConfig>(File.ReadAllText("creds.json"));
if (smtpConfig is null)
{
    throw new Exception("Could not read SMTP config");
}

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddSignalR();

ApiConfig? credentials = JsonSerializer.Deserialize<ApiConfig>(File.ReadAllText("creds.json"));
if (credentials is null)
{
    throw new Exception("Could not read API credentials");
}

builder.Services
    .AddScoped<IAuthService,
        AuthService>();
builder.Services
    .AddScoped<IUserRoleService,
        UserRoleService>();
builder.Services
    .AddScoped<IUserService,
        UserService>();
builder.Services.AddSingleton(smtpConfig);
builder.Services.AddSingleton<IBackgroundEmailNotificationQueue, BackgroundEmailNotificationQueue>();
builder.Services.AddSingleton<IListingService, ListingService>();
builder.Services.AddSingleton<IQuoteService, QuoteService>();
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient(HttpClientNames.FrontEnd, client =>
{
    if (builder.Configuration["FrontEndUrl"] is null)
    {
        throw new Exception("FrontEndUrl is not set");
    }

    client.BaseAddress = new Uri(builder.Configuration["FrontEndUrl"] ?? String.Empty);
});
builder.Services.AddHttpClient(HttpClientNames.CoinMarketApi, client =>
{
    client.BaseAddress = new Uri("https://pro-api.coinmarketcap.com");
    client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", credentials.CoinMarketApiKey);
    client.DefaultRequestHeaders.Add("Accepts", "application/json");
});

builder.Services.AddSingleton<CoinMarketApiClient>();
builder.Services.AddQuartz(q =>
{
    q.ScheduleJob<UpdateListingsJob>(trigger => trigger
        .WithIdentity("UpdateListingsJob-Trigger")
        .WithDailyTimeIntervalSchedule(x => x.WithInterval(10, IntervalUnit.Second))
        .WithDescription("Updates listing for all clients every 10 seconds")
    );
});
builder.Services.AddSingleton<IListingsMemoryCache, ListingsMemoryCache>();
builder.Services.AddSingleton<IListingsMemoryCacheService, ListingsMemoryCacheService>();
builder.Services.AddHostedService<FrontendHealthCheckHostedService>();
builder.Services.AddHostedService<ApiListingCacheHostedService>();
builder.Services.AddHostedService<EmailNotificationBackgroundService>();
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        if (builder.Configuration["Jwt:Key"] == null) throw new Exception("Jwt:Key is not set in appsettings.json");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "secret"))
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole(UserRole.UserRoleNames[UserRoleName.Admin]));
    options.AddPolicy("User", policy => policy.RequireRole(UserRole.UserRoleNames[UserRoleName.User]));
});
builder.Services.AddDbContext<DataContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

var app = builder.Build();
app.UseCors(builder =>
{
    builder.WithOrigins("http://localhost:5173");
    builder.AllowAnyMethod();
    builder.AllowAnyHeader();
    builder.AllowCredentials();
});
app.UseAuthorization();
app.UseExceptionHandling();
app.MapControllers();
app.MapHub<CurrencyHub>("/currencyHub");

app.Run();