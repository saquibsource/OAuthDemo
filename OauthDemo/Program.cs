using WebAPIApplication.Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Add services to the container.
IConfigurationRoot configuration = new ConfigurationBuilder().AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: false).Build();

builder.Services.AddAppSettingsModule(configuration);
builder.Services.AddSecurityModule();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseApplicationSecurity();

app.MapControllers();

app.Run();