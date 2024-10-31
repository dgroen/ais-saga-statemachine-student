using System.Net.Mime;
using MassTransit;
using MassTransit.AzureServiceBusTransport;
using Microsoft.Extensions.DependencyInjection;

namespace MessageBrokers
{
    public class ASB
    {

        public static IBusControl ConfigureBus(
            IServiceProvider serviceProvider, Action<IServiceBusBusFactoryConfigurator, IServiceBusHost> action = null)
        {
            return Bus.Factory.CreateUsingAzureServiceBus(cfg =>
            {
                // cfg.Host(ASBConfig.ASBURL);
                cfg.Host(new Uri(ASBConfig.ASBURL))
                cfg.ConfigureEndpoints(serviceProvider.GetRequiredService<IBusRegistrationContext>());
                // Added the next two lines for Azure Service Bus to support JSON payloads
                cfg.DefaultContentType = new ContentType("application/json");
                cfg.UseRawJsonDeserializer();
                cfg.ConfigureEndpoints(serviceProvider.GetRequiredService<IBusRegistrationContext>());
            });

        }
    }
}
