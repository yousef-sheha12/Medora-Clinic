using System.Text.Json.Serialization;
using MedoraClinic.API.Exceptions;
using MedoraClinic.API.Extensions;
using MedoraClinic.API.Filters;
using MedoraClinic.Application;
using MedoraClinic.Application.Common;
using MedoraClinic.Infrastructure;
using MedoraClinic.Infrastructure.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

DotEnvLoader.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
    options.Filters.Add<FluentValidationFilter>())
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .SelectMany(entry => entry.Value?.Errors.Select(error => error.ErrorMessage)
                ?? Enumerable.Empty<string>())
            .ToArray();

        return new BadRequestObjectResult(ApiResponse<object>.Failure(
            "الطلب غير صالح.",
            errors,
            StatusCodes.Status400BadRequest));
    };
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddApiAuthentication(builder.Configuration);
builder.Services.AddOpenApi();

builder.Services.AddCors(options => options.AddPolicy("Frontend", policy =>
{
    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
}));

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("Frontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
