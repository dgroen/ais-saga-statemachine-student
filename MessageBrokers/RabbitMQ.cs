using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.DependencyInjection;

namespace MessageBrokers
{
    public class RabbitMQ
    {
        public static IBusControl ConfigureBus(
            IServiceProvider serviceProvider, Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> action = null)
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri(RabbitMQConfig.RabbitMQURL), hst =>
                {
                    hst.Username(RabbitMQConfig.UserName);
                    hst.Password(RabbitMQConfig.Password);
                });
                
                cfg.ConfigureEndpoints(serviceProvider.GetRequiredService<IBusRegistrationContext>());
            });
        }
    }
}
