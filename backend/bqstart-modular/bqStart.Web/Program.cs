using bqStart.Data;
using Microsoft.EntityFrameworkCore;
using Serilog.Events;
using Serilog;
using System.Configuration;
using BinaryQuest.Framework.ModularCore;
using BinaryQuest.Framework.ModularCore.Security;
using Microsoft.AspNetCore.OData;
using bqStart.Web.Controllers;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console(),true);

// Add services to the container.
builder.Services.AddDbContext<MainDataContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: false));

builder.Services.AddCors(opt => {
    opt.AddDefaultPolicy(opt => opt.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

//BQ Admin related
builder.Services.AddBqAdminServices<MainDataContext>(options =>
    options.SetApplicationName("BQ Start")
    .SetSecurityRulesProvider(new FileBasedSecurityRulesProvider("config"))
    .RegisterController<Department, DepartmentController>()
    .RegisterController<ExampleClass, ExampleClassController>()
    .RegisterController<Order, OrderController>()
    );

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//TODO::
//Setup email sending services here
//builder.Services.Configure<EmailSenderOptions>(options => builder.Configuration.GetSection("ExternalProviders:SMTP").Bind(options));
//builder.Services.AddTransient<IEmailSender, EmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseODataRouteDebug();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors();

//load all middlewares we need from the framework
//and register the endpoints we will need for data
app.UseBQAdmin<MainDataContext>().Build();

Log.Information("Starting up!");

app.Run();
