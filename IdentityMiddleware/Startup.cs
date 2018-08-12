using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IdentityMiddleware.IdentityProvider;
using IdentityMiddleware.IdentityProvider.Model;
using IdentityModel;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Configuration;
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
            
            //添加MS的Identity实现
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
            

            //添加实现Ms的Identity实现中需要用到的自定义存储实现
            string connectionString = Configuration.GetConnectionString("MySqlConnection");
            services.AddTransient<MySqlConnection>(e => new MySqlConnection(connectionString));
            services.AddTransient<UserTable>();
            services.AddTransient<RoleTable>();
            services.AddTransient<IUserStore<UserModel>, UserStore>();
            services.AddTransient<IUserClaimStore<UserModel>, UserStore>();
            services.AddTransient<IUserRoleStore<UserModel>, UserStore>();
            services.AddTransient<IUserPasswordStore<UserModel>, UserStore>();
            services.AddTransient<IRoleStore<RoleModel>, RoleStore>();

            //添加IdentityServer4
            services.AddIdentityServer()
                //添加储存和test
                .AddDeveloperSigningCredential()
                .AddInMemoryPersistedGrants()
                //添加资源
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                //添加客户端
                .AddInMemoryClients(Config.GetClients())
                //添加用户
                .AddTestUsers(Config.GetUsers())
                .AddAspNetIdentity<UserModel>();
            
            services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;//"Bearer"
                        options.DefaultChallengeScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;//"Bearer"
                        options.DefaultForbidScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;//"Bearer"
                        //options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;//"Bearer"
                    })
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "UserManger";
                    options.RoleClaimType = JwtClaimTypes.Role;
                    options.NameClaimType = JwtClaimTypes.Name;
                    //options.ApiSecret = "secret1";
                    //options.SupportedTokens = SupportedTokens.Both;
                });

            services.AddAuthorization();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
        }
    }
}
