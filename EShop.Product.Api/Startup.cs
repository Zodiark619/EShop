using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EShop.Infrastructure.EventBus;
using EShop.Infrastructure.Mongo;
using EShop.Product.Api.Handlers;
using EShop.Product.Api.Repositories;
using EShop.Product.Api.Services;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace EShop.Product.Api
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
            

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EShop.Product.Api", Version = "v1" });
            });
            services.AddMongoDb(Configuration);
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<CreateProductHandler>();

            var rabbitmqOption = new RabbitMqOption();
            Configuration.GetSection("rabbitmq").Bind(rabbitmqOption);
            services.AddMassTransit(x =>
            {
                x.AddConsumer<CreateProductHandler>();

                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(new Uri(rabbitmqOption.ConnectionString), hostconfig =>
                    {
                        hostconfig.Username(rabbitmqOption.Username);
                        hostconfig.Password(rabbitmqOption.Password);

                    });
                    cfg.ReceiveEndpoint("create_product", ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(retryConfig =>
                        {
                            retryConfig.Interval(2, 100);
                        });
                        ep.ConfigureConsumer<CreateProductHandler>(provider);
                    });
                }));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EShop.Product.Api v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            var dbInitializer=app.ApplicationServices.GetService<IDatabaseInitializer>();
            dbInitializer.InitializeAsync();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            var busControl = app.ApplicationServices.GetService<IBusControl>();
            busControl.Start();
        }
    }
}
