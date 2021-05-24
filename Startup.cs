using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using Newtonsoft.Json;
using UserManagement.Models;
using Microsoft.AspNetCore.Authentication;
using UserManagement.Handlers;

namespace UserManagement
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
            services.AddDbContext<UserManagementDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("UserManagementConn")));

            // services.AddScoped<IUserManagement, MockUserManagement>();
            services.AddScoped<IUserManagement, SqlUserManagement>();
            services.AddScoped<IUserAuthentication, UserAuthentication>();

            services.AddControllers()
                .AddNewtonsoftJson();

            services.AddAuthentication("CustomAuth")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuth", null)
                .AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>("CustomAuth", null);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UserManagement", Version = "v1" });
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UserManagement v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
