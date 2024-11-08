using MassTransit;
using SagaStateMachine;
using Microsoft.EntityFrameworkCore;
using SagaService.Models;
using MessageBrokers;
using System.Net.Mime;
using MassTransit.Middleware;
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

        var connectionString = Configuration.GetConnectionString("DbConnection");
        var sagaQueueName = Configuration.GetValue<string>("SagaQueueName");
        BrokerTypes brokerType = (BrokerTypes)Enum.Parse(typeof(BrokerTypes),
            Configuration.GetValue<string>("BrokerType"));


        services.AddDbContextPool<SagaContext>(db => db.UseSqlServer(connectionString));

        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            switch (brokerType)
            {
                case BrokerTypes.ASB:
                    x.UsingAzureServiceBus((_, cfg) =>
                    {
                        cfg.Host(Configuration.GetConnectionString("AzureServiceBus"));
                        cfg.ReceiveEndpoint(sagaQueueName, ep =>
                        {
                            ep.PrefetchCount = 10;
                        });
                        cfg.DefaultContentType = new ContentType("application/json");
                        cfg.UseRawJsonDeserializer();
                        cfg.ConfigureEndpoints(_.GetRequiredService<IBusRegistrationContext>());
                    });
                    break;
                case BrokerTypes.RabbitMQ:
                    x.UsingRabbitMq((_, cfg) =>
                    {
                        cfg.Host(Configuration.GetConnectionString("RabbitMQ")); 
                        cfg.ReceiveEndpoint(sagaQueueName, ep =>
                        {
                            ep.PrefetchCount = 1;
                            ep.ConfigureDeadLetter(x =>
                            {
                                x.UseFilter(new DeadLetterTransportFilter());
                            });

                        });
                        cfg.ConfigureEndpoints(_.GetRequiredService<IBusRegistrationContext>());
                    });
                    break;
                default:
                    throw new NotImplementedException();
            }
            x.AddSagaStateMachine<StudentStateMachine, StudentStateData>()
                .EntityFrameworkRepository(r =>
                {
                    r.ConcurrencyMode = ConcurrencyMode.Pessimistic; // or use Optimistic, which requires RowVersion
                    r.ExistingDbContext<SagaContext>();
                });
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