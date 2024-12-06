using Azure.Security.KeyVault.Secrets;
using Moq;
using System;
using Xunit;
using Util.Azure;
using Azure;

namespace RegisterStudent.Util.Azure.Tests
{
    public class KeyVaultClientTests
    {
        // [Fact]
        // public void GetSecretFromUri_WithValidUri_ReturnsSecretValue()
        // {
        //     // Arrange
        //     var secretUri = new Uri("https://myvault.vault.azure.net/secrets/mysecret");
        //     var expectedSecretValue = "secretValue";
        //     var mockSecretClient = new Mock<SecretClient>();

        //     mockSecretClient.Setup(client => client.GetSecret(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
        //     .Returns(Response.FromValue(new KeyVaultSecret("mysecret", expectedSecretValue), null));
        //     var keyVaultClient = new KeyVaultClient();

        //     // Act
        //     var actualSecretValue = KeyVaultClient.GetSecretFromUri(secretUri);

        //     // Assert
        //     Assert.Equal(expectedSecretValue, actualSecretValue);
        // }

        // [Fact]
        // public void GetSecretFromUri_WithInvalidUri_ThrowsUriFormatException()
        // {
        //     // Arrange
        //     var invalidUri = "invalidUri";

        //     // Act & Assert
        //     Assert.Throws<UriFormatException>(() => KeyVaultClient.GetSecretFromUri(invalidUri));
        // }

        // [Fact]
        // public void GetSecretFromName_WithValidInputs_ReturnsSecretValue()
        // {
        //     // Arrange
        //     var keyVaultURL = "https://myvault.vault.azure.net";
        //     var secretName = "mysecret";
        //     var expectedSecretValue = "secretValue";
        //     var mockSecretClient = new Mock<SecretClient>();
        //     mockSecretClient.Setup(client => client.GetSecret(secretName))
        //         .Returns(Response.FromValue(new KeyVaultSecret(secretName, expectedSecretValue), null));
        //     var keyVaultClient = new KeyVaultClient();

        //     // Act
        //     var actualSecretValue = KeyVaultClient.GetSecretFromName(keyVaultURL, secretName);

        //     // Assert
        //     Assert.Equal(expectedSecretValue, actualSecretValue);
        // }

        // [Fact]
        // public void GetSecretFromName_WithInvalidKeyVaultURL_ThrowsUriFormatException()
        // {
        //     // Arrange
        //     var invalidKeyVaultURL = "invalidURL";
        //     var secretName = "mysecret";

        //     // Act & Assert
        //     Assert.Throws<UriFormatException>(() => KeyVaultClient.GetSecretFromName(invalidKeyVaultURL, secretName));
        // }

        // [Fact]
        // public void GetSecretFromName_WithNonExistentSecret_ThrowsRequestFailedException()
        // {
        //     // Arrange
        //     var keyVaultURL = "https://myvault.vault.azure.net";
        //     var nonExistentSecretName = "nonexistentsecret";
        //     var mockSecretClient = new Mock<SecretClient>();
        //     mockSecretClient.Setup(client => client.GetSecret(nonExistentSecretName))
        //         .Throws(new RequestFailedException("Secret not found"));
        //     var keyVaultClient = new KeyVaultClient();

        //     // Act & Assert
        //     Assert.Throws<RequestFailedException>(() => KeyVaultClient.GetSecretFromName(keyVaultURL, nonExistentSecretName));
        // }
    }
}
