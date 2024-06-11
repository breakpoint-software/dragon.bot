using Binance.Net;
using CryptoExchange.Net.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddBinance(options =>
{
    // Options can be configured here, for example:
    options.ApiCredentials = new ApiCredentials(builder.Configuration.GetSection("Binance:ApiKey").Value ?? "", builder.Configuration.GetSection("Binance:Secret").Value ?? "");
    options.Environment = BinanceEnvironment.Testnet;
});


var app = builder.Build();

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
