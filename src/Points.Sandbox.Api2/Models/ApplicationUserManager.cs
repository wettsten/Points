using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AspNet.Identity.RavenDB.Stores;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Points.Data;
using Raven.Client;

namespace Points.Api2.Models
{
    public class ApplicationUserManager : UserManager<User>
    {
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var session = context.Get<IAsyncDocumentSession>();
            session.Advanced.UseOptimisticConcurrency = true;
            var manager = new ApplicationUserManager(new RavenUserStore<User>(session));

            manager.UserValidator = new UserValidator<User>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 4,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"));
            }

            return manager;
        }

        public ApplicationUserManager(IUserStore<User> store) : base(store)
        {
        }
    }
}