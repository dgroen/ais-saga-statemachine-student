using MassTransit;
using StudentService.Consumers;
using Microsoft.EntityFrameworkCore;
using StudentService.Models;
using StudentService.Services;
using MessageBrokers;
using Microsoft.OpenApi.Models;
using System.Reflection;

public class Startup
{
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration, which is used
        /// to configure the application services.</param>
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

        /// <summary>
        /// Configures the application services.
        /// </summary>
        /// <param name="services">The collection of services to add to the ASP.NET Core dependency injection container.</param>
        ///
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
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Student Service API", Version = "v1" });
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });
    }

    /// <summary>
    /// Configures the HTTP request pipeline for the application.
    /// </summary>
    /// <param name="app">Provides the mechanisms to configure an application's request pipeline.</param>
    /// <param name="env">Provides information about the web hosting environment an application is running in.</param>
    /// <remarks>
    /// In development environment, it enables the developer exception page and Swagger UI.
    /// It also sets up HTTPS redirection, routing, authorization, and maps the controller endpoints.
    /// </remarks>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Student Service API v1");
            });
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