using FluentAssertions;
using Jordnaer.Server.Database;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Jordnaer.Server.Tests;

[Trait("Category", "IntegrationTest")]
[Collection(nameof(JordnaerServerFactory))]
public class AzureAppConfiguration_Should
{
    private readonly WebApplicationFactory<Program> _factory;

    public AzureAppConfiguration_Should(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public void Contain_Authentication_Scheme_Facebook()
    {
        // Arrange
        var facebookOptions = new FacebookOptions();
        var configuration = _factory.Services.GetRequiredService<IConfiguration>();

        // Act
        configuration.GetSection("Authentication:Schemes:Facebook").Bind(facebookOptions);

        // Assert
        facebookOptions.AppId.Should().NotBeEmpty();
        facebookOptions.AppSecret.Should().NotBeEmpty();
        facebookOptions.SaveTokens.Should().BeTrue();
    }

    [Fact]
    public void Contain_Authentication_Scheme_Microsoft()
    {
        // Arrange
        var microsoftAccountOptions = new MicrosoftAccountOptions();
        var configuration = _factory.Services.GetRequiredService<IConfiguration>();

        // Act
        configuration.GetSection("Authentication:Schemes:Microsoft").Bind(microsoftAccountOptions);

        // Assert
        microsoftAccountOptions.ClientId.Should().NotBeEmpty();
        microsoftAccountOptions.ClientSecret.Should().NotBeEmpty();
        microsoftAccountOptions.SaveTokens.Should().BeTrue();
    }

    [Fact]
    public void Contain_Authentication_Scheme_Google()
    {
        // Arrange
        var facebookOptions = new GoogleOptions();
        var configuration = _factory.Services.GetRequiredService<IConfiguration>();

        // Act
        configuration.GetSection("Authentication:Schemes:Google").Bind(facebookOptions);

        // Assert
        facebookOptions.ClientId.Should().NotBeEmpty();
        facebookOptions.ClientSecret.Should().NotBeEmpty();
        facebookOptions.SaveTokens.Should().BeTrue();
    }

    [Fact]
    public void Contain_ConnectionString_JordnaerDbContext()
    {
        // Arrange
        var configuration = _factory.Services.GetRequiredService<IConfiguration>();

        // Act
        string? connectionString = configuration.GetConnectionString(nameof(JordnaerDbContext));

        // Assert
        connectionString.Should()
            .Contain("Server")
            .And.ContainAny("Database", "Initial Catalog")
            .And.Contain("User Id")
            .And.Contain("Password")
            .And.Contain("TrustServerCertificate");
    }
}
