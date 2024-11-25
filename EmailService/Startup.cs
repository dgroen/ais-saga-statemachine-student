using MassTransit;
using EmailService.Consumers;
using Microsoft.EntityFrameworkCore;
using MessageBrokers;

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
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            x.AddConsumer<SendEmailConsumer>();

            BrokerTypes brokerType = (BrokerTypes)Enum.Parse(typeof(BrokerTypes),
            Configuration.GetValue<string>("BrokerType"));
            switch (brokerType)
            {
                case BrokerTypes.ASB:
                    x.UsingAzureServiceBus((context, cfg) =>
                    {
                        cfg.Host(Configuration.GetConnectionString("AzureServiceBus"));
                        cfg.ConfigureEndpoints(context);
                    });

                    break;
                case BrokerTypes.RabbitMQ:
                    x.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.Host(Configuration.GetConnectionString("RabbitMQ"));
                        cfg.ConfigureEndpoints(context);
                    });
                    break;
                default:
                    break;
            }

        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }

}