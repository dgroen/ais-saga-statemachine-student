using MassTransit;
using StudentService.Consumers;
using Microsoft.EntityFrameworkCore;
using StudentService.Models;
using StudentService.Services;
using MessageBrokers;
using Microsoft.OpenApi.Models;

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
        services.AddScoped<IStudentServices, StudentServices>();

        // Auto Mapper Configurations
        services.AddAutoMapper(typeof(Startup));

        var connectionString = Configuration.GetConnectionString("DbConnection");
        var sagaQueueName = Configuration.GetValue<string>("SagaQueueName");
        services.AddDbContextPool<AppDbContext>(db => db.UseSqlServer(connectionString));

        BrokerTypes brokerType = (BrokerTypes)Enum.Parse(typeof(BrokerTypes),
        Configuration.GetValue<string>("BrokerType"));

        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            x.AddConsumer<GetValueConsumer>();
            x.AddConsumer<RegisterStudentCancelConsumer>();

            switch (brokerType)
            {
                case BrokerTypes.ASB:
                    x.UsingAzureServiceBus((_, cfg) =>
                    {
                        cfg.Host(Configuration.GetConnectionString("AzureServiceBus"));
                        cfg.ReceiveEndpoint(sagaQueueName, ep =>
                        {
                            ep.PrefetchCount = 10;
                            // Get Consumer
                            ep.ConfigureConsumer<GetValueConsumer>(_);
                            ep.ConfigureConsumer<RegisterStudentCancelConsumer>(_);
                        });
                    });
                    break;
                case BrokerTypes.RabbitMQ:
                    x.UsingRabbitMq((_, cfg) =>
                    {
                        cfg.Host(Configuration.GetConnectionString("RabbitMQ"));
                        cfg.ReceiveEndpoint(sagaQueueName, ep =>
                        {
                            ep.PrefetchCount = 10;
                            // Get Consumer
                            ep.ConfigureConsumer<GetValueConsumer>(_);
                            ep.ConfigureConsumer<RegisterStudentCancelConsumer>(_);
                        });
                    });
                    break;
                default:
                    break;
            }
        });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
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