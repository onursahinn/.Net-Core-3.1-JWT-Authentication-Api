using Authentication.Helpers;
using Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Core;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Context.Entities;

namespace Authentication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();
            services.AddHttpContextAccessor();

            services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<CurrentContext>()
    .AddDefaultTokenProviders();


            HttpHelper.conStrSql = Configuration.GetConnectionString(Configuration["AppSettings:ActiveBranch"]);
            string assemblyNameDB = typeof(CurrentContext).Namespace;
            services.AddDbContext<Context.CurrentContext>(options =>
            {
                options.UseSqlServer(HttpHelper.conStrSql, sqlServerOptions => { sqlServerOptions.CommandTimeout(10200); sqlServerOptions.MigrationsAssembly(assemblyNameDB); });
                
            });
            services.AddTransient<IUserService, UserService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
          .AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());

            app.UseMiddleware<JwtMiddleWare>();

            HttpHelper.Configure(httpContextAccessor, Configuration);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseEndpoints(x => x.MapControllers());
        }
    }
}
