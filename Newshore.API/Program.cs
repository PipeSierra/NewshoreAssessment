using DataAccess;

using Newshore.API.Filter;
using Newshore.Business.Contracts;
using Newshore.Business.Models;
using Newshore.Business.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200")
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                      });
});

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<HttpResponseExceptionFilter>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IHttpClientService, HttpClientService>();
builder.Services.AddScoped<IJourneyService, JourneyService>();
builder.Services.AddScoped<IFlightService, FlightService>();

builder.Services.Configure<Settings>(builder.Configuration.GetSection("CustomSettings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseExceptionHandler("/error-development");
}
else
    app.UseExceptionHandler("/error");

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();
