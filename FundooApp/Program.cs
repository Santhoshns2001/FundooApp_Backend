using System.Text;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Service;
using DataAcessLayer.Data;
using DataAcessLayer.Interface;
using DataAcessLayer.Repositary;
using EmailService.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using EmailService.Services;
using EmailService.Consumers;
using Serilog;
using FundooApp.MiddleWare;

var builder = WebApplication.CreateBuilder(args);

//  Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(
        path: "Logs/app-.log",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7
    )
    .CreateLogger();


// CORS 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


//  Tell ASP.NET Core to use Serilog
builder.Host.UseSerilog();

// ---------------- Controllers + JSON FIX ----------------

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling =
            ReferenceLoopHandling.Ignore;
    });





// ---------------- JWT ----------------
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            )
        };
    });

builder.Services.AddAuthorization();

// ---------------- EF Core ----------------
builder.Services.AddDbContext<FundooDBContext>(options =>
    options.UseSqlServer(
        builder.Configuration["ConnectionStrings:FunDooConnections"]
    )
);

// ---------------- Dependency Injection ----------------
builder.Services.AddScoped<IUserRegisterBuss, UserBuss>();
builder.Services.AddScoped<IUserRepo, UserRepo>();

builder.Services.AddScoped<IJWTService, JwtService>();

builder.Services.AddScoped<INotesBuss, NotesBuss>();
builder.Services.AddScoped<INotesRepo, NotesRepo>();

builder.Services.AddScoped<IEmailService, EmailService.Services.EmailService>();

builder.Services.AddScoped<ILabelBuss, LabelBuss>();
builder.Services.AddScoped<ILabelRepo,LabelRepo>();

builder.Services.AddScoped<ICollabBuss, CollabBuss>();
builder.Services.AddScoped<ICollabRepo, CollabRepo>();

builder.Services.AddSingleton<IRabbitMqProducer, RabbitMqProducer>();

builder.Services.AddHostedService<ForgotPasswordConsumer>();



// ---------------- Redis Cache ----------------
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration =
        builder.Configuration.GetValue<string>("RedisCacheOptions:Configuration")
        ?? "localhost:6379";

    options.InstanceName =
        builder.Configuration.GetValue<string>("RedisCacheOptions:InstanceName")
        ?? "FundooRedisCache:";
});

// ---------------- Swagger + JWT ----------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FundooNotes API",
        Version = "v1"
    });

    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter JWT token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// ---------------- Build ----------------
var app = builder.Build();

//----------Custom Middleware------------------
app.UseMiddleware<GlobalExceptionMiddleware>();


app.UseCors("AllowAngularApp");


// ---------------- Pipeline ----------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
