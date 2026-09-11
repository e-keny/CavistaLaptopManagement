using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Features.Shared;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Models;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Services;
using Duende.IdentityModel.Client;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CavistaLaptopLifecycleManagement.Api.Features.Users.Endpoints.Queries
{
    public static partial class GetAssetUrl
    {
        public record Query ([FromForm] IFormFile [] data);

        public sealed record GetAssetUrlResponse(string? token, bool isSuccess, string? message);

        private async static ValueTask<Ok<GetAssetUrlResponse>> HandleAsync(
            Query request,
            UserService userService,
            CLMDbContext context,
            IOptions<AppSettings> options,
            CancellationToken token)
        {
            var appSetting = options.Value;           

            return TypedResults.Ok(new GetAssetUrlResponse("", true, "successful"));
        }
    }
}
