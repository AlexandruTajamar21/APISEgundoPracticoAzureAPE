using APISEgundoPracticoAzureAPE.Data;
using APISEgundoPracticoAzureAPE.Helpers;
using APISEgundoPracticoAzureAPE.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APISEgundoPracticoAzureAPE
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
            string azureSql =
                        this.Configuration.GetConnectionString("azureSql");
            services.AddTransient<RepositoryTickets>();
            services.AddTransient<RepositoryUsuarios>();
            services.AddDbContext<AzureContext>
                (options => options.UseSqlServer(azureSql));
            HelperOAuthToken helper = new HelperOAuthToken(this.Configuration);
            services.AddAuthentication(helper.GetAuthenticationOptions())
                .AddJwtBearer(helper.GetJwtOptions());
            services.AddTransient<HelperOAuthToken>
                (x => helper);
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Api Empleados OAuth",
                    Version = "v1"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "APISEgundoPracticoAzureAPE v1");
                c.RoutePrefix = "";
            });

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
