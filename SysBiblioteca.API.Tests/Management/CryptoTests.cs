using Xunit;
using FluentAssertions;
using SysBiblioteca.API.Management;

namespace SysBiblioteca.API.Tests.Management;

public class CryptoTests
{
    [Fact]
    public void Encrypt_TextoPlano_RetornaTextoDistinto()
    {
        var encrypted = crypto.Encrypt("texto");

        encrypted.Should().NotBeNullOrWhiteSpace();
        encrypted.Should().NotBe("texto");
    }

    [Fact]
    public void Decrypt_TextoEncriptado_RetornaTextoOriginal()
    {
        var encrypted = crypto.Encrypt("texto");

        var decrypted = crypto.Decrypt(encrypted);

        decrypted.Should().Be("texto");
    }
}
