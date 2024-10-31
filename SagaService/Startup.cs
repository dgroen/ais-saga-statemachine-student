using MassTransit;
using SagaStateMachine;
using Microsoft.EntityFrameworkCore;
using SagaService.Models;
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
        services.AddDbContextPool<SagaContext>(db => db.UseSqlServer(connectionString));

        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            x.AddBus(provider=> MessageBrokers.ASB.ConfigureBus(provider));
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