using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using WordFrequencyService.Core.Services;
using WordFrequencyService.Data;
using WordFrequencyService.Data.Contexts;
using WordFrequencyService.Data.Repositories;

namespace WordFrequencyService.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextFactory<WordFrequencySqlDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddSingleton<IWordFrequencyRepository, WordFrequencySqlRepository>();
            services.AddSingleton<IContentFetcher, ContentFetcher>();
            services.AddSingleton<IWordFrequencyCalculatorService, WordFrequencyCalculatorService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Word Frequency Service",
                    Version = "v1",
                    Description = "An API with an endpoint that accepts a URL to any website (e.g., Wikipedia or BBC News) in order to fetch the content of that URL and build a dictionary that contains the frequency of use of each word on that page. Each time a URL is being fetched, it saves the top 100 most frequent words to a database (MSSQL). In case of an existent word from previous fetches, the frequency is updated with the additional frequency calculated. The API has another endpoint which returns the top 100 words from its database, where the response is ordered by the most frequent word towards to the less frequent word.",
                    Contact = new OpenApiContact
                    {
                        Name = "Marton Kis-Varday",
                        Email = "kisvardaymarton@icloud.com",
                        Url = new Uri("https://github.com/maatonmies")
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Word Frequency Service V1");
            });

            app.UseExceptionHandler(builder => builder.Run(async httpContext =>
            {
                var exceptionHandlerPathFeature = httpContext.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature.Error;

                var json = JsonConvert.SerializeObject(new { error = exception.Message });
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsync(json);
            }));

            app.UseHsts();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
