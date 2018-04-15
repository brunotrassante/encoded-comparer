using EncodedComparer.Domain.Handlers;
using EncodedComparer.Domain.Repository;
using EncodedComparer.Infra.DataContexts;
using EncodedComparer.Infra.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;

namespace EncodedComparer
{
    public class Startup
    {
        public static string ConnectionString { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            ConnectionString = Configuration.GetConnectionString("EncodedComparerConnection");
            services.AddScoped(_ => new EncodedComparerContext(ConnectionString));
            services.AddTransient<IEncodedPairRepository, EncodedPairRepository>();
            services.AddTransient<EncodedPairHandler, EncodedPairHandler>();

            services.AddMvc();

            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new Info { Title = "EncodedComparer", Version = "v1" });
                //var basePath = AppContext.BaseDirectory;
                //var xmlPath = Path.Combine(basePath, "EncodedComparer.API.xml");
                //s.IncludeXmlComments(xmlPath);
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseRewriter(new RewriteOptions().AddRedirect("^$", "swagger"));

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");

            });
        }
    }
}
