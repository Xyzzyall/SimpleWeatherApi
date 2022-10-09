using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Serilog;
using SimpleWeatherApi.ApiClients.OpenWeather;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLazyCache();

builder.Host.UseSerilog(
    new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger()
);

builder.Services.AddSingleton<IOpenWeatherClient, OpenWeatherClient>();
builder.Services.AddHttpClient<IOpenWeatherClient, OpenWeatherClient>(
    client => client.BaseAddress = new Uri("https://api.openweathermap.org")
).AddPolicyHandler(HttpPolicyExtensions
    .HandleTransientHttpError()
    .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(
        medianFirstRetryDelay: TimeSpan.FromSeconds(1), retryCount: 5
    ))
);
builder.Services.Decorate<IOpenWeatherClient, CachedOpenWeatherClient>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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