using CavistaLaptopLifecycleManagement.Api;
using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Features.Shared;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Services;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Services.Requirements;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Security.Claims;


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


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContextPool<CLMDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DBConnection")));

builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection(AppSettings.AppSettingSection));

_ = builder.Services.AddCavistaLaptopLifecycleManagementApiServices();

_ = builder.Services.AddCavistaLaptopLifecycleManagementApiHandlers();

_ = builder.Services.AddMemoryCache();

_ = builder.Services.AddHttpContextAccessor();

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
    app.MapOpenApi();

    app.MapScalarApiReference();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "API V1");
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
