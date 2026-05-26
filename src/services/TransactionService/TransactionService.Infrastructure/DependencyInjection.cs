using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TransactionService.Application.Common;
using TransactionService.Domain.Interfaces;
using TransactionService.Infrastructure.Messaging;
using TransactionService.Infrastructure.Persistence;
using TransactionService.Infrastructure.Persistence.Repositories;

namespace TransactionService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<TransactionDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IMessagePublisher, MessagePublisher>();

        services.AddMassTransit(x =>
           {
               x.UsingRabbitMq((context, cfg) =>
               {
                   cfg.Host(configuration["RabbitMQ:Host"], "/", h =>
                   {
                       h.Username(configuration["RabbitMQ:Username"]!);
                       h.Password(configuration["RabbitMQ:Password"]!);
                   });
               });
           });

        services.AddStackExchangeRedisCache(options =>
           {
               options.Configuration = configuration["Redis:ConnectionString"];
           });   

        return services;
    }
}