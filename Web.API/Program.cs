using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using RelatoX.Application.DTOs;
using RelatoX.Application.Interfaces;
using RelatoX.Application.Validators;
using RelatoX.Application.Validators.Queries;
using RelatoX.Infra.Data.InMemory;
using RelatoX.Infra.Middlewares;
using RelatoX.Persistence;
using RelatoX.Persistence.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

TimeZoneInfo brazilTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
DateTime nowInBrazil = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, brazilTimeZone);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddScoped<IConsumptionRepository, InMemoryConsumptionRepository>();
builder.Services.AddScoped<IConsumptionService, ConsumptionService>();

#region FluentValidation

// Registra todos os validadores automaticamente do assembly
builder.Services.AddValidatorsFromAssemblyContaining<ConsumptionPostDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ConsumptionQueryValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ReportQueryValidator>();

// Ou registra manualmente (caso prefira)
builder.Services.AddScoped<IValidator<ConsumptionPostDto>, ConsumptionPostDtoValidator>();

builder.Services
    .AddControllers()
    //.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ConsumptionPostDtoValidator>())
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ReportQueryValidator>())
.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ConsumptionQueryValidator>());

#endregion FluentValidation

#region DbContext

var str = builder.Configuration["Postgresql:ConnectionString"];

builder.Services.AddDbContext<ApplicationDbContext>(options =>
     options
         .UseNpgsql(str)
         .UseSnakeCaseNamingConvention());

#endregion DbContext

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    //options.MapType<DateTimeOffset>(() => new OpenApiSchema
    //{
    //    Type = "string",
    //    Format = "date-time",
    //    Example = new OpenApiString(DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:sszzz"))
    //});
});

var app = builder.Build();

#region Middlewares

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ErrorHandlingMiddleware>();

#endregion Middlewares

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

//summary
//comments swaegger
//tests