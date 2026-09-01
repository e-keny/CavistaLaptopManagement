using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace TestCavistaIdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("scope1"),
                new ApiScope("scope2"),
                new ApiScope("verification"),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientId = "m2m.client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                    AllowedScopes = { "scope1" }
                },

                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "interactive",
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = { "https://localhost:44300/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "scope2" }
                },

                new Client
                    {
                        ClientId = "web",
                        ClientSecrets = { new Secret("secret".Sha256()) },

                        AllowedGrantTypes = GrantTypes.Code,

                        // where to redirect to after login
                       RedirectUris = 
                        {  "http://interactivewebclient.onrender.com/signin-oidc",
                           "https://localhost:44300/signin-oidc",          // for local dev
                           "https://interactivewebclient.onrender.com/signin-oidc", // for Render deployment
                           "https://laptrac-woad.vercel.app/signin-oidc",
                           "http://laptrac-woad.vercel.app/signin-oidc",
                           "https://laptrac-woad.vercel.app/login",
                           "http://laptrac-woad.vercel.app/login",
                           "https://laptrac-woad.vercel.app/login/signin-oidc",
                           "http://laptrac-woad.vercel.app/login/signin-oidc"
                        },

                       // RedirectUris = { "https://localhost:44300/signin-oidc" },

                        // where to redirect to after logout
                        PostLogoutRedirectUris = {
                            "https://interactivewebclient.onrender.com/signout-callback-oidc",
                            "https://laptrac-woad.vercel.app/signout-callback-oidc",
                            "http://laptrac-woad.vercel.app/signout-callback-oidc",
                            "https://laptrac-woad.vercel.app/login",
                            "http://laptrac-woad.vercel.app/login",
                            "https://laptrac-woad.vercel.app/login/signout-callback-oidc",
                            "http://laptrac-woad.vercel.app/login/signout-callback-oidc"
                        },

                        RequirePkce = true,
                        AllowOfflineAccess = true,

                        AllowedScopes =
                        {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            "verification",
                            "scope1",
                            "scope2"
                        }
                    }
            };
    }
}
