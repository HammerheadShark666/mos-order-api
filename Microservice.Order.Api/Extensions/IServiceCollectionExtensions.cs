// Ignore Spelling: Mediatr Api Grpc Jwt Versioning

using Asp.Versioning;
using Azure.Identity;
using FluentValidation;
using Grpc.Core;
using Grpc.Net.Client.Configuration;
using MediatR;
using Microservice.Order.Api.Data.Context;
using Microservice.Order.Api.Data.Repository;
using Microservice.Order.Api.Data.Repository.Interfaces;
using Microservice.Order.Api.Grpc;
using Microservice.Order.Api.Grpc.Interfaces;
using Microservice.Order.Api.Helpers;
using Microservice.Order.Api.Helpers.Automapper;
using Microservice.Order.Api.Helpers.Interfaces;
using Microservice.Order.Api.Helpers.Swagger;
using Microservice.Order.Api.MediatR.AddOrder;
using Microservice.Order.Api.Middleware;
using Microservice.Order.Api.Protos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Microservice.Order.Api.Extensions;

public static class IServiceCollectionExtensions
{
    public static void ConfigureExceptionHandling(this IServiceCollection services)
    {
        services.AddTransient<ExceptionHandlingMiddleware>();
    }

    public static void ConfigureJwt(this IServiceCollection services)
    {
        services.AddJwtAuthentication();
    }

    public static void ConfigureDI(this IServiceCollection services)
    {
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<ICustomerAddressService, CustomerAddressService>();
        services.AddScoped<IJwtHelper, JwtHelper>();
        services.AddScoped<IOrderHelper, OrderHelper>();
        services.AddScoped<IAzureServiceBusHelper, AzureServiceBusHelper>();
        services.AddMemoryCache();
        services.AddHttpContextAccessor();
        services.AddSingleton<ICustomerHttpAccessor, CustomerHttpAccessor>();
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    }

    public static void ConfigureAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetAssembly(typeof(AutoMapperProfile)));
    }

    public static void ConfigureDatabaseContext(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContextFactory<OrderDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString(Helpers.Constants.DatabaseConnectionString)));
    }

    public static void ConfigureMediatr(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<AddOrderValidator>();
        services.AddMediatR(_ => _.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
    }

    public static void ConfigureApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("X-Api-Version"));
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen(options =>
        {
            options.OperationFilter<SwaggerDefaultValues>();
            options.SupportNonNullableReferenceTypes();
        });
    }

    public static void ConfigureGrpc(this IServiceCollection services)
    {
        var retryPolicy = new MethodConfig
        {
            Names = { MethodName.Default },
            RetryPolicy = new RetryPolicy
            {
                MaxAttempts = 5,
                InitialBackoff = TimeSpan.FromSeconds(1),
                MaxBackoff = TimeSpan.FromSeconds(0.5),
                BackoffMultiplier = 1,
                RetryableStatusCodes = { StatusCode.Internal }
            }
        };

        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        services.AddGrpcClient<BookGrpc.BookGrpcClient>
                (
                    o => { o.Address = new Uri(EnvironmentVariables.GRPC_Book_Url); }
                )
                .ConfigureChannel(o =>
                {
                    o.HttpHandler = handler;
                    o.ServiceConfig = new ServiceConfig()
                    {
                        MethodConfigs = {
                            retryPolicy
                        }
                    };
                })
                .AddCallCredentials((context, metadata, serviceProvider) =>
                {
                    var provider = serviceProvider.GetRequiredService<IJwtHelper>();
                    var token = provider.GenerateJwtToken();
                    metadata.Add("Authorization", $"Bearer {token}");
                    return Task.CompletedTask;
                });
    }

    public static void ConfigureServiceBusClient(this IServiceCollection services, bool isDevelopment)
    {
        if (isDevelopment)
        {
            services.AddAzureClients(builder =>
            {
                builder.AddServiceBusClient(EnvironmentVariables.GetEnvironmentVariable(Constants.AzureServiceBusConnection));
            });
        }
        else
        {
            services.AddAzureClients(builder =>
            {
                builder.AddServiceBusClientWithNamespace(EnvironmentVariables.GetEnvironmentVariable(Constants.AzureServiceBusConnectionManagedIdentity));
                builder.UseCredential(new ManagedIdentityCredential());
            });
        }
    }
}