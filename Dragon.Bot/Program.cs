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
    options.ApiCredentials = new ApiCredentials("XsCQcHF7tByImp4yKq6SCSvimYikdkdNtYtETKzAyhd8by8jHl0mslxYnkNlk4v3", "dz70WyYj903l6FzP00GTRyyia3MtNvEcUDvaq7AYGjnYRhpNI6X5d9CzRMm8pezz");
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
