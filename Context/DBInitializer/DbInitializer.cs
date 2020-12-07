using Context.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace Context
{
    public static class DbInitializer
    {
        public static void Initialize(CurrentContext context, IServiceProvider serviceProvider)
        {
            context.Database.Migrate();
            var userMan = (UserManager<User>)serviceProvider.GetService(typeof(UserManager<User>));
            if (userMan.Users.Count()==0)
            {
                var task1 = userMan.CreateAsync(new User
                {
                    Email = $"admin@mail.com",
                    UserName = "admin",
                    Name = "Test",
                    LastName = "Test",
                    IsActive = true,
                    IsDeleted = false,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                }, "123Abc.");
                task1.Wait();
                context.SaveChangesAsync();
            }
        }
        public static IHost HandleDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<CurrentContext>())
                {
                    try
                    {
                        Initialize(appContext, scope.ServiceProvider);
                    }
                    catch (Exception ex)
                    {
                        //Log errors or do anything you think it's needed
                        throw;
                    }
                }
            }
            return host;
        }
    }
}
