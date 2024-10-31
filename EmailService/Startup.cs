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
            x.UsingAzureServiceBus((_, cfg) =>
            {
                cfg.Host(Configuration.GetConnectionString("AzureServiceBus"));
                cfg.ReceiveEndpoint(ASBQueues.SagaBusQueue, ep =>
                {
                    ep.PrefetchCount = 10;
                    // Get Consumer
                    ep.ConfigureConsumer<SendEmailConsumer>(_);
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