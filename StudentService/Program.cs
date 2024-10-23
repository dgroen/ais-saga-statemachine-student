using MassTransit;
using Microsoft.EntityFrameworkCore;
using StudentService.Consumers;
using StudentService.Models;
using StudentService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Connection string
var connectionString = builder.Configuration.GetConnectionString("DbConnection");
builder.Services.AddDbContextPool<AppDbContext>(db => db.UseSqlServer(connectionString));

// Registe MassTransit
builder.Services.AddMassTransit(cfg =>
{
    cfg.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
    {
        cfg.ReceiveEndpoint(MessageBrokers.RabbitMQQueues.SagaBusQueue, ep =>
        {
            ep.PrefetchCount = 10;
            // Get Consumer
            ep.ConfigureConsumer<GetValueConsumer>(provider);
            // Cancel Consumer
            ep.ConfigureConsumer<RegisterStudentCancelConsumer>(provider);
        });
    }));

    cfg.AddConsumer<GetValueConsumer>();
    cfg.AddConsumer<RegisterStudentCancelConsumer>();
});

// Register Student Service
builder.Services.AddScoped<IStudentServices, StudentServices>();

// Register AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
