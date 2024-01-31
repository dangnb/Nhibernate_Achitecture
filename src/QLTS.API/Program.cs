using QLTS.API.DependencyInjection.Extensions;
using QLTS.API.Middleware;
using QLTS.Persistence.DependencyInjection.Extentions;
using Serilog;
using QLTS.Application.DependencyInjection.Extensions;
using NHibernate.Cfg;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen();
// configure strongly typed settings object
builder.Services.Configure<QLTS.Contract.AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddCustomJwtAuthentication(builder.Configuration);
Log.Logger = new LoggerConfiguration().ReadFrom
    .Configuration(builder.Configuration)
    .CreateLogger();

builder.Logging
    .ClearProviders()
    .AddSerilog();
// đăng ký middleware để quản lý scope
builder.Services.AddConfigureMediatR();

builder.Services.AddConfigureAutoMapper();


// Add services to the container.

builder.Services.AddSwagger();

builder.Services.AddControllers().AddApplicationPart(QLTS.Persentation.AssemblyReference.assembly);

builder.Services.AddTransient<ExceptionHandlingMiddleware>();
;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.InitializeContainer(builder.Configuration);
builder.Services.InitializeContext();

builder.Services
    .AddApiVersioning(options => options.ReportApiVersions = true)
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });
var app = builder.Build();

app.UseMiddleware<WindsorScopeMiddleware>();

app.UseMiddleware<ExceptionHandlingMiddleware>();
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
if (builder.Environment.IsDevelopment() || builder.Environment.IsStaging())
{
    app.ConfigureSwagger();
}

try
{
    await app.RunAsync();
    Log.Information("Stopped cleanly");
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
    await app.StopAsync();
}
finally
{
    Log.CloseAndFlush();
    await app.DisposeAsync();
}
