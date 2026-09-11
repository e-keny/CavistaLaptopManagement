using CavistaLaptopLifecycleManagement.Api;
using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Features.Shared;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Services;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Services.Requirements;
using CavistaLaptopLifecycleManagement.Api.Infrastructure.Exceptions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Security.Claims;
using Microsoft.OpenApi.Models;



var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddAuthentication()
    .AddJwtBearer(option =>
    {
        option.Authority = configuration["IdentityServer:Authority"];
        //option.Authority = "https://localhost:5001";
        option.TokenValidationParameters.ValidateAudience = false;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.ITRolePolicy, policy =>
       policy.Requirements.Add(new ITRoleRequirement(ClaimTypes.NameIdentifier)));
});


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cavista Laptop Management", Version = "v1" });

    // Define the Bearer scheme
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your JWT token."
    });

    // Apply the scheme globally
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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


builder.Services.AddDbContextPool<CLMDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DBConnection")));

builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection(AppSettings.AppSettingSection));

_ = builder.Services.AddCavistaLaptopLifecycleManagementApiServices();

_ = builder.Services.AddCavistaLaptopLifecycleManagementApiHandlers();

_ = builder.Services.AddMemoryCache();

_ = builder.Services.AddHttpContextAccessor();

_ = builder.Services.AddProblemDetails(ExceptionStartupExtensions.ConfigureProblemDetails);

var policyName = "CorsPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: policyName, builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{  
    app.MapScalarApiReference();
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Management API v1");
    });
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseCors(policyName);

_ = app.UseRouting();

_ = app.InitializeDatabase();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapCavistaLaptopLifecycleManagementApiEndpoints();
});

app.Run();
