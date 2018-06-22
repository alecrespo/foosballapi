using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Ecovadis.API.Infrastructure.Middlewares;
using Ecovadis.API.Services;
using Ecovadis.DAL.Contexts;
using Ecovadis.DAL.Models;
using Ecovadis.DL.Infrastructures;
using Ecovadis.DL.Repositories;
using Ecovadis.DL.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace Ecovadis.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", true, true)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Automapper
            services.AddAutoMapper();
            //Entity Framework
            services.AddDbContext<EcovadisContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //Error Handler
            services.AddTransient<IErrorHandler, ErrorHandler>();
            //Unit of work
            ////services.AddTransient<IRepository<Assignment>, GenericRepository<Assignment>>(); 
            //Game Logic
            services.AddTransient<ILogicService, LogicService>();
            //Game Service
            services.AddTransient<IGameService, GameService>();
            //Cors [Only for tests, later this can be set to whitelist]
            services.AddCors(options =>
            {
                options.AddPolicy(
                          "AllowAllOrigins",
                          builder =>
                          {
                              builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                          });
            });
            //Swagger
            ConfigureServiceSwagger(services);
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            ConfigureAppSwagger(app);
            app.UseMvc();
        }
        private void ConfigureAppSwagger(IApplicationBuilder app)
        {

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DocExpansion(DocExpansion.None);
                c.RoutePrefix = "swagger";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ecovadis API V1");
            });
        }
        private void ConfigureServiceSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                          new Info
                          {
                              Title = "Ecovadis API",
                              Version = "v1",
                              Description = "Ecovadis Foosball API",
                              Contact = new Contact() { Name = "Alejandro Crespo", Email = "alejandro.crespo@gmail.com" },
                              TermsOfService = "Shareware",
                              License = new License()
                              {
                                  Name = "MIT",
                                  Url = "https://opensource.org/licenses/MIT"
                              }
                          }
                       );
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.DescribeAllEnumsAsStrings();
            });
        }
    }
}
