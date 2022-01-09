using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Confluent.Kafka;
using FluentMigrator.Runner;
using FluentValidation.AspNetCore;
using KafkaMessageBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using Repository.Ifaces;
using Repository.Migration;
using Repository.Models;

namespace Repository
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<IProductRepository, PostgresRepository>();
            services.AddTransient<IDbConnection>(db => new NpgsqlConnection(Configuration.GetConnectionString("PostgresConnection")));

            services.AddLogging(c => c.AddFluentMigratorConsole())
                .AddFluentMigratorCore()
                .ConfigureRunner(r => r
                    .AddPostgres()
                    .WithGlobalConnectionString(Configuration.GetConnectionString("PostgresConnection"))
                    .ScanIn(Assembly.GetEntryAssembly()).For.Migrations()
                );
            

            services.AddKafkaConsumer<string, Product, KafkaMessagesHandler>(p =>
            {
                p.Topic = "update_db";
                p.GroupId = "database";
                p.BootstrapServers = "localhost:9092";
            });
            
            services.AddControllers()
                .AddFluentValidation(s =>
                {
                    s.RegisterValidatorsFromAssemblyContaining<Startup>();
                });
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Migrate();
        }
    }
}