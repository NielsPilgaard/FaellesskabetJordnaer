using Jordnaer.Server.Consumers;
using Jordnaer.Server.Database;
using MassTransit;
using Microsoft.AspNetCore.ResponseCompression;

namespace Jordnaer.Server.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddMassTransit(this WebApplicationBuilder builder)
    {
        builder.Services.AddMassTransit(config =>
        {
            config.AddConsumer<StartChatConsumer>();
            config.AddConsumer<SendMessageConsumer>();
            config.AddConsumer<SetChatNameConsumer>();

            if (builder.Environment.IsDevelopment())
            {
                config.SetEndpointNameFormatter(endpointNameFormatter: new DefaultEndpointNameFormatter(prefix: "dev-"));
            }

            config.UsingAzureServiceBus((context, azureServiceBus) =>
            {
                if (builder.Environment.IsDevelopment())
                {
                    azureServiceBus
                        .MessageTopology
                        .SetEntityNameFormatter(
                            new PrefixEntityNameFormatter(AzureBusFactory.MessageTopology.EntityNameFormatter, "dev-"));
                }

                azureServiceBus.Host(builder.Configuration.GetConnectionString("AzureServiceBus"));

                azureServiceBus.ConfigureEndpoints(context);
            });
        });

        return builder;
    }

    public static WebApplicationBuilder AddAzureSignalR(this WebApplicationBuilder builder)
    {
        builder.Services.AddSignalR(options =>
        {
            options.ClientTimeoutInterval = TimeSpan.FromMinutes(5);
            options.KeepAliveInterval = TimeSpan.FromMinutes(1);
        });
        //.AddAzureSignalR(options =>
        //{
        //    options.ConnectionString = builder.Configuration.GetConnectionString("AzureSignalR");
        //    options.ServerStickyMode = ServerStickyMode.Required;
        //});

        if (!builder.Environment.IsDevelopment())
        {
            builder.Services.AddResponseCompression(options =>
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" }));
        }

        return builder;
    }

    public static WebApplicationBuilder AddDatabase(this WebApplicationBuilder builder)
    {
        string dbConnectionString = GetConnectionString(builder.Configuration);

        builder.Services.AddSqlServer<JordnaerDbContext>(dbConnectionString);

        builder.Services.AddHealthChecks().AddSqlServer(dbConnectionString);

        return builder;
    }

    private static string GetConnectionString(IConfiguration configuration) =>
           Environment.GetEnvironmentVariable($"ConnectionStrings_{nameof(JordnaerDbContext)}")
        ?? Environment.GetEnvironmentVariable($"ConnectionStrings__{nameof(JordnaerDbContext)}")
        ?? configuration.GetConnectionString(nameof(JordnaerDbContext))
        ?? throw new InvalidOperationException(
            $"Connection string '{nameof(JordnaerDbContext)}' not found.");
}
