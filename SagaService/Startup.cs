using MassTransit;
using SagaStateMachine;
using Microsoft.EntityFrameworkCore;
using SagaService.Models;
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

        var connectionString = Configuration.GetConnectionString("DbConnection");
        BrokerTypes brokerType = (BrokerTypes)Enum.Parse(typeof(BrokerTypes),
            Configuration.GetValue<string>("BrokerType"));


        services.AddDbContextPool<SagaContext>(db => db.UseSqlServer(connectionString));

        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            switch (brokerType)
            {
                case BrokerTypes.ASB:
                    x.AddBus(provider => MessageBrokers.ASB.ConfigureBus(provider));
                    break;
                case BrokerTypes.RabbitMQ:
                    x.AddBus(provider => MessageBrokers.RabbitMQ.ConfigureBus(provider));
                    break;
                default:
                    x.AddBus(provider => MessageBrokers.RabbitMQ.ConfigureBus(provider));
                    break;
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