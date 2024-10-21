using Binance.Net;
using CryptoExchange.Net.Authentication;
using Dragon.Bot;
using DragonBot.Services.Interfaces;
using DragonBot.Services.Profiles;
using DragonBot.Services.Services;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ClientIpCheckActionFilter>(container =>
{
    var loggerFactory = container.GetRequiredService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger<ClientIpCheckActionFilter>();

    return new ClientIpCheckActionFilter(builder.Configuration["AdminSafeList"]);
});

//builder.Services.AddScoped<IpBlockingMiddleware>();
builder.Services.AddAutoMapper(typeof(OrderProfile));


builder.Services.AddBinance(options =>
{
    // Options can be configured here, for example:
    options.ApiCredentials = new ApiCredentials(builder.Configuration.GetSection("Binance:ApiKey").Value ?? "", builder.Configuration.GetSection("Binance:Secret").Value ?? "");
    options.Environment = builder.Environment.IsDevelopment() ? BinanceEnvironment.Testnet : BinanceEnvironment.Live;
});

builder.Services.AddDbContext<DragonBotDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DragonBot");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddScoped<ISignalHandlerService, BinanceSignalHandlerService>();
builder.Services.AddScoped<IAssetProvider, BinanceProvider>();

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions() { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.All });

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DragonBotDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
