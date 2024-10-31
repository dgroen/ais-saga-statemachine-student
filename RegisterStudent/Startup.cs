using MassTransit;
using RegisterStudent.Consumers;
using Microsoft.EntityFrameworkCore;
using RegisterStudent.Models;
using RegisterStudent.Services;
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
        services.AddScoped<IStudentInfoService, StudentInfoService>();

        // Auto Mapper Configurations
        services.AddAutoMapper(typeof(Startup));

        var connectionString = Configuration.GetConnectionString("DbConnection");
        services.AddDbContextPool<AppDbContext>(db => db.UseSqlServer(connectionString));

        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            x.AddConsumer<RegisterStudentConsumer>();
            x.AddConsumer<CancelSendingEmailConsumer>();

            x.UsingAzureServiceBus((_, cfg) =>
            {
                cfg.Host(Configuration.GetConnectionString("AzureServiceBus"));
                cfg.ReceiveEndpoint(ASBQueues.SagaBusQueue, ep =>
                {
                    ep.PrefetchCount = 10;
                    // Get Consumer
                    ep.ConfigureConsumer<RegisterStudentConsumer>(_);
                    ep.ConfigureConsumer<CancelSendingEmailConsumer>(_);
                });
            });
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            // app.UseOpenApi();
            // app.UseSwaggerUi();
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