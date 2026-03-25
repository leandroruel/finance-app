using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.SecretsManager;
using Amazon.SimpleEmail;
using Amazon.SQS;
using FinanceApp.Application.Common.Interfaces;
using FinanceApp.Infrastructure.Persistence;
using FinanceApp.Infrastructure.Persistence.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        services.AddMessaging(configuration);
        services.AddAwsServices(configuration);
        services.AddRepositories();

        return services;
    }

    private static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<FinanceDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                npgsql => npgsql.MigrationsAssembly(typeof(FinanceDbContext).Assembly.FullName)));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<FinanceDbContext>());

        return services;
    }

    private static IServiceCollection AddMessaging(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var sqsEndpoint = configuration["AWS:SQSEndpoint"] ?? "http://localhost:4566";

        services.AddMassTransit(x =>
        {
            x.AddEntityFrameworkOutbox<FinanceDbContext>(o =>
            {
                o.UsePostgres();
                o.UseBusOutbox();
            });

            x.UsingAmazonSqs((ctx, cfg) =>
            {
                cfg.Host("us-east-1", h =>
                {
                    h.AccessKey(configuration["AWS:AccessKey"] ?? "test");
                    h.SecretKey(configuration["AWS:SecretKey"] ?? "test");
                    h.Config(new AmazonSQSConfig
                    {
                        ServiceURL = sqsEndpoint
                    });
                });

                cfg.ConfigureEndpoints(ctx);
            });
        });

        return services;
    }

    private static IServiceCollection AddAwsServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var awsEndpoint = configuration["AWS:Endpoint"] ?? "http://localhost:4566";
        var credentials = new BasicAWSCredentials(
            configuration["AWS:AccessKey"] ?? "test",
            configuration["AWS:SecretKey"] ?? "test");
        var region = RegionEndpoint.USEast1;

        services.AddSingleton<IAmazonS3>(_ => new AmazonS3Client(
            credentials, new AmazonS3Config { ServiceURL = awsEndpoint, ForcePathStyle = true }));

        services.AddSingleton<IAmazonSecretsManager>(_ => new AmazonSecretsManagerClient(
            credentials, new AmazonSecretsManagerConfig { ServiceURL = awsEndpoint }));

        services.AddSingleton<IAmazonSimpleEmailService>(_ => new AmazonSimpleEmailServiceClient(
            credentials, new AmazonSimpleEmailServiceConfig { ServiceURL = awsEndpoint }));

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();

        return services;
    }
}
