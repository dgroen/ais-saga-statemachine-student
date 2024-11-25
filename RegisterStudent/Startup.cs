using MassTransit;
using RegisterStudent.Consumers;
using Microsoft.EntityFrameworkCore;
using RegisterStudent.Models;
using RegisterStudent.Services;
using MessageBrokers;
using Microsoft.Extensions.DependencyInjection;

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
        
        // var sagaQueueName = Configuration.GetValue<string>("SagaQueueName");
        services.AddControllers();
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.AddConsumer<RegisterStudentConsumer>();
            x.AddConsumer<CancelSendingEmailConsumer>();
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
                    x.UsingRabbitMq((context,cfg) =>
                    {
                        cfg.Host(Configuration.GetConnectionString("RabbitMQ"));
                        cfg.ConfigureEndpoints(context);
                    });
                    break;
                default:
                    break;
            }
        });        
        services.AddScoped<IStudentInfoService, StudentInfoService>();

        // Auto Mapper Configurations
        services.AddAutoMapper(typeof(Startup));

        var connectionString = Configuration.GetConnectionString("DbConnection");
        services.AddDbContextPool<AppDbContext>(db => db.UseSqlServer(connectionString));



        
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