using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Features.Shared;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Models;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Services;
using Duende.IdentityModel.Client;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;


namespace CavistaLaptopLifecycleManagement.Api.Features.Users.Endpoints.Queries
{
    [Handler]
    [MapGet("access-token")]
    [MapGroup<UserMapGroup>]
    [AllowAnonymous]
    public static partial class GetAccessToken
    {
        public record Query();

        public sealed record AccessTokenResponse( string? token, bool isSuccess, string? message);

        private async static ValueTask<Ok<AccessTokenResponse>> HandleAsync(
            Query _,
            UserService userService,
            CLMDbContext context,
            IOptions<AppSettings> options,
            CancellationToken token)
        {
            var appSetting = options.Value;

            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(appSetting.IdentityAddress);

            if (disco != null && disco.IsError)
            {
                Console.WriteLine(disco.Error);
                Console.WriteLine(disco.Exception);

                return TypedResults.Ok(new AccessTokenResponse(disco?.Error, false, disco?.Exception?.Message));
            }

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "m2m.client",
                ClientSecret = "511536EF-F270-4058-80CA-1C89C192F69A",
                Scope = "scope1"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(disco.Error);
                Console.WriteLine(disco.Exception);

                return TypedResults.Ok(new AccessTokenResponse(disco?.Error, false, disco?.Exception?.Message));
            }

            return TypedResults.Ok(new AccessTokenResponse(tokenResponse.AccessToken, true, "successful"));
        }
    }
}
