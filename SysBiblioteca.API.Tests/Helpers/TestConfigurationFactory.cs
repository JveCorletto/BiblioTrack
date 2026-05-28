using Microsoft.Extensions.Configuration;

namespace SysBiblioteca.API.Tests.Helpers;

public static class TestConfigurationFactory
{
    public static IConfiguration CreateJwtConfiguration()
    {
        var values = new Dictionary<string, string?>
        {
            ["JWT:JWT_EXPIRE_MINUTES"] = "60",
            ["JWT:JWT_EXPIRE_DAYS_SERVICES"] = "1",
            ["JWT:JWT_ISSUER_TOKEN"] = "SysBiblioteca.Tests",
            ["JWT:JWT_AUDIENCE_TOKEM"] = "SysBiblioteca.Tests.Client",
            ["JWT:JWT_SUBJECT_TOKEN"] = "SysBiblioteca.Tests.Subject",
            ["JWT:JWT_SECRET_KEY"] = "SysBiblioteca.Tests.Secret.Key.With.More.Than.32.Chars"
        };

        return new ConfigurationBuilder().AddInMemoryCollection(values).Build();
    }
}
