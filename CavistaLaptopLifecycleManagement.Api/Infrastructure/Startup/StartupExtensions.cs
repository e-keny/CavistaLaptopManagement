using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace CavistaLaptopLifecycleManagement.Api.Infrastructure.Startup
{
    public static class StartupExtensions
    {      
        public static IEndpointRouteBuilder MapAccountServices(this IEndpointRouteBuilder app)
        {
            _ = app
                .MapGet("/Login", async (HttpContext context, string returnUrl = "/") =>
                {
                    var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                        .WithRedirectUri(returnUrl)
                        .Build();

                    await context.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
                });

            _ = app
                .MapGet("/Logout", async (HttpContext context, string returnUrl = "/") =>
                {
                    var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
                        .WithRedirectUri(returnUrl)
                        .Build();

                    await context.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
                    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                })
                .RequireAuthorization();

            return app;
        }
    }
}
