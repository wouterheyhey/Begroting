using Microsoft.Owin;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebApi.Providers;
using Microsoft.AspNet.Identity;

[assembly: OwinStartup(typeof(WebApi.Startup))]
namespace WebApi
{
    public class Startup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
        public static GoogleOAuth2AuthenticationOptions googleAuthOptions { get; private set; }
        public static FacebookAuthenticationOptions facebookAuthOptions { get; private set; }


        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            //hier maken we een cookie die zal dienen als tijdelijke storage voor gebruikers die via een 3rd part tool inloggen
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();

            googleAuthOptions = new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "xxx",
                ClientSecret = "xxx",
                Provider = new GoogleAuthorizationProvider()
            };

            facebookAuthOptions = new FacebookAuthenticationOptions()
            {
                AppId = "xxx",
                AppSecret = "xxx",
                Provider = new FacebookAuthorizationProvider()
            };

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = new AccountAuthorizationServerProvider(),
                RefreshTokenProvider = new AccountRefreshTokenProvider()
            };

            // Hier maken we de token aan
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }


    }
}

