using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using DeliveryUpdater.Data;
using DeliveryUpdater.Ifaces;
using DeliveryUpdater.Logic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using KafkaMessageBus;
using Microsoft.Extensions.Configuration;

namespace DeliveryUpdater
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDeliveryUpdaterLogic, DeliveryUpdaterLogic>();
            
            services.AddControllers();

            services.AddKafkaMessageBus();

            services.AddKafkaProducer<string, MyProduct>(p =>
            {
                p.Topic = "update_db";
                p.BootstrapServers = "localhost:9092";
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
        }
    }
}