using System;
using System.Data.Entity;
using System.Web.Http;
using Aminjam.Owin.Security.Instagram;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.MicrosoftAccount;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Twitter;
using Owin;
using Points.Api.Auth;
using Points.Api.Auth.Migrations;
using Points.Api.Auth.Providers;

[assembly: OwinStartup(typeof(Startup))]

namespace Points.Api.Auth
{
    public class Startup
    {
        public static OAuthBearerAuthenticationOptions oAuthBearerOptions { get; private set; }
        public static GoogleOAuth2AuthenticationOptions googleAuthOptions { get; private set; }
        public static FacebookAuthenticationOptions facebookAuthOptions { get; private set; }
        public static TwitterAuthenticationOptions twitterAuthOptions { get; private set; }
        public static MicrosoftAccountAuthenticationOptions microsoftAuthOptions { get; private set; }
        public static InstagramAuthenticationOptions instagramAuthOptions { get; private set; }
        
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            ConfigureOAuth(app);

            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AuthContext, Configuration>());

        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            //use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);
            oAuthBearerOptions = new OAuthBearerAuthenticationOptions();

            var oAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(12),
                Provider = new SimpleAuthorizationServerProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(oAuthBearerOptions);

            //Configure Google External Login
            googleAuthOptions = new GoogleOAuth2AuthenticationOptions
            {
                ClientId = "1064139911926-bopakjt93anenu7v15o66kesd1u63k30.apps.googleusercontent.com",
                ClientSecret = "QfJidaAe1IP-AWWukhAgKT6Q",
                Provider = new GoogleAuthProvider()
            };
            app.UseGoogleAuthentication(googleAuthOptions);

            //Configure Facebook External Login
            facebookAuthOptions = new FacebookAuthenticationOptions
            {
                AppId = "792287044238273",
                AppSecret = "13f33fb4b2f8e84a3d63e0a6cd845b0e",
                Provider = new FacebookAuthProvider()
            };
            app.UseFacebookAuthentication(facebookAuthOptions);

            //Configure Twitter External Login
            twitterAuthOptions = new TwitterAuthenticationOptions
            {
                ConsumerKey = "bgnLCSWbqTdzlUjfaA1Bn0oEp",
                ConsumerSecret = "8joy0Bv4Lqo199ZlxuXjJgH8CTJ93kxtgc0ak0jzk1YiyRTeuc",
                Provider = new TwitterAuthProvider(),
                BackchannelCertificateValidator = new CertificateSubjectKeyIdentifierValidator(new[]
                {
                    "A5EF0B11CEC04103A34A659048B21CE0572D7D47", // VeriSign Class 3 Secure Server CA - G2
                    "0D445C165344C1827E1D20AB25F40163D8BE79A5", // VeriSign Class 3 Secure Server CA - G3
                    "7FD365A7C2DDECBBF03009F34339FA02AF333133", // VeriSign Class 3 Public Primary Certification Authority - G5
                    "39A55D933676616E73A761DFA16A7E59CDE66FAD", // Symantec Class 3 Secure Server CA - G4
                    "5168FF90AF0207753CCCD9656462A212B859723B", //DigiCert SHA2 High Assurance Server C‎A 
                    "B13EC36903F8BF4701D498261A0802EF63642BC3" //DigiCert High Assurance EV Root CA
                })
            };
            app.UseTwitterAuthentication(twitterAuthOptions);

            //Configure Microsoft External Login
            microsoftAuthOptions = new MicrosoftAccountAuthenticationOptions()
            {
                ClientId = "000000004018730F",
                ClientSecret = "BOGjr1WiRqQf9dBETNCS0aRZnCKj0Sst",
                Provider = new MicrosoftAuthProvider()
            };
            app.UseMicrosoftAccountAuthentication(microsoftAuthOptions);

            //Configure Instagram External Login
            instagramAuthOptions = new InstagramAuthenticationOptions()
            {
                ClientId = "62960fd93d134c9ab7e74227cfdef46b",
                ClientSecret = "d5923e7f62214538b9c3fd74aec3df2a",
                Provider = new InstagramAuthProvider()
            };
            app.UseInstagramAuthentication(instagramAuthOptions);
        }
    }

}