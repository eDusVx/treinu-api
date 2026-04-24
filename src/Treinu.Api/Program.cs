using System.Text;
using System.Text.Json.Serialization;
using Microsoft.OpenApi;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Treinu.Api.Middlewares;
using Treinu.Application.Extensions;
using Treinu.Application.Handlers.Treinadores;
using Treinu.Application.Interfaces;
using Treinu.Domain.Core;
using Treinu.Domain.Factories;
using Treinu.Domain.Factories.Interfaces;
using Treinu.Domain.Repositories;
using Treinu.Infrastructure.Data;
using Treinu.Infrastructure.Health;
using Treinu.Infrastructure.Repositories;
using Treinu.Infrastructure.Security;
using Treinu.Infrastructure.Data.Repositories;
using Treinu.Api.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.AllowOutOfOrderMetadataProperties = true;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("bearer", document)] = []
    });
});

var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
                       ?? builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found. Set environment variable 'ConnectionStrings__DefaultConnection'.");

builder.Services.AddPostgresHealthCheck(connectionString);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ICredencialRepository, CredencialRepository>();
builder.Services.AddScoped<IUsuarioFactory, UsuarioFactory>();
builder.Services.AddScoped<AvaliacaoFisicaFactory>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IConviteRepository, ConviteRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ITemplateEmailRepository, TemplateEmailRepository>();
builder.Services.AddScoped<ITreinoRepository, TreinoRepository>();
builder.Services.AddScoped<IExercicioRepository, ExercicioRepository>();
builder.Services.AddScoped<INotificacaoRepository, NotificacaoRepository>();

builder.Services.AddHostedService<TreinosVencidosWorker>();

builder.Services.AddValidatorsFromAssembly(typeof(RegistrarTreinadorHandler).Assembly);
builder.Services.AddCustomMediator(typeof(RegistrarTreinadorHandler).Assembly);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secret = jwtSettings.GetValue<string>("Secret") ?? "DefaultSecretKeyss";

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
            ValidAudience = jwtSettings.GetValue<string>("Audience"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
        };
    });

var app = builder.Build();

app.UseExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapCustomHealthChecks();

app.Run();