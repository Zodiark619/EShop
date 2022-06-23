using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EShop.Infrastructure.EventBus
{
    public static class Extension
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services,IConfiguration configuration)
        {
            var rabbitMq = new RabbitMqOption();
            configuration.GetSection("rabbitmq").Bind(rabbitMq);

            services.AddMassTransit(x =>
            {
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(new Uri(rabbitMq.ConnectionString), hostcfg =>
                    {
                        hostcfg.Username(rabbitMq.Username);
                        hostcfg.Password(rabbitMq.Password);

                    });
                }));
            });
            return services;
        }
    }
}
