using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityMiddleware.IdentityProvider;
using IdentityMiddleware.IdentityProvider.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;

namespace IdentityMiddleware
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddIdentity<UserModel, RoleModel>(options =>
                {
                    // Password settings.
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 6;
                    //options.Password.RequiredUniqueChars = 1;

                    // Lockout settings.
                    //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    //options.Lockout.MaxFailedAccessAttempts = 5;
                    //options.Lockout.AllowedForNewUsers = true;

                    // User settings.
                    options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                    //options.User.RequireUniqueEmail = false;
                })
                .AddDefaultTokenProviders();

            string connectionString = Configuration.GetConnectionString("MySqlConnection");
            services.AddTransient<MySqlConnection>(e => new MySqlConnection(connectionString));
            services.AddTransient<UserTable>();
            services.AddTransient<RoleTable>();
            services.AddTransient<IUserStore<UserModel>, UserStore>();
            services.AddTransient<IUserClaimStore<UserModel>, UserStore>();
            services.AddTransient<IUserRoleStore<UserModel>, UserStore>();
            services.AddTransient<IUserPasswordStore<UserModel>, UserStore>();
            services.AddTransient<IRoleStore<RoleModel>, RoleStore>();
            
            
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryPersistedGrants()
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddTestUsers(Config.GetUsers())
                .AddAspNetIdentity<UserModel>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseIdentityServer();
            app.UseMvcWithDefaultRoute();
        }
    }
}
