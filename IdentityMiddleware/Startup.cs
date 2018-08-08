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
            services.AddIdentity<UserModel, RoleModel>()
                .AddDefaultTokenProviders();
            
            services.AddTransient<IUserStore<UserModel>, UserStore>();
            services.AddTransient<IUserClaimStore<UserModel>, UserStore>();
            services.AddTransient<IUserRoleStore<UserModel>, UserStore>();
            services.AddTransient<IUserPasswordStore<UserModel>, UserStore>();
            services.AddTransient<IRoleStore<RoleModel>, RoleStore>();
            string connectionString = Configuration.GetConnectionString("MySqlConnection");
            services.AddTransient<MySqlConnection>(e => new MySqlConnection(connectionString));

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
            app.UseIdentityServer();

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
