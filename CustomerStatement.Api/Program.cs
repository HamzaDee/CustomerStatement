using CustomerStatement.Api.Exceptions;
using CustomerStatement.Application.Behaviors;
using CustomerStatement.Application.Features.Statements.Queries;
using CustomerStatement.Application.Interfaces;
using CustomerStatement.Infrastructure.Data;
using CustomerStatement.Infrastructure.Repositories;
using CustomerStatement.Infrastructure.Services;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CustomerStatement.Api.Swagger.Examples;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Customer Statement API",
        Version = "v1",
        Description = "API for generating and retrieving monthly customer account statements."
    });

    c.ExampleFilters();
}); 
builder.Services.AddSwaggerExamplesFromAssemblyOf<GenerateMonthlyStatementCommandExample>();
builder.Services.AddScoped<IStatementRepository, StatementRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetStatementsQueryHandler).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(GetStatementsQueryValidator).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>),typeof(ValidationBehavior<,>));
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();
app.UseExceptionHandler();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();