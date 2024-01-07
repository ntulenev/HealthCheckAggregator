using Abstractions.Transport;

using Microsoft.Extensions.Logging;

namespace Logic.Tests;

public class ResourceCheckerTests
{
    [Fact(DisplayName = $"{nameof(ResourceChecker)} can be created with valid params")]
    [Trait("Category", "Unit")]
    public void ResourceCheckerCanBeCreatedWithValidParams()
    {
        // Arrange
        var rawChecker = new Mock<IRawResourceChecker>(MockBehavior.Strict).Object;
        var logger = new Mock<ILogger<ResourceChecker>>(MockBehavior.Strict).Object;

        // Act
        var exception = Record.Exception(() =>
            _ = new ResourceChecker(rawChecker, logger));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"{nameof(ResourceChecker)} can't be created without raw checker")]
    [Trait("Category", "Unit")]
    public void ResourceCheckerCantBeCreatedWithoutRawChecker()
    {
        // Arrange
        var logger = new Mock<ILogger<ResourceChecker>>(MockBehavior.Strict).Object;
        // Act
        var exception = Record.Exception(() =>
            _ = new ResourceChecker(null!, logger));
        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(ResourceChecker)} can't be created without logger")]
    [Trait("Category", "Unit")]
    public void ResourceCheckerCantBeCreatedWithoutLogger()
    {
        // Arrange
        var rawChecker = new Mock<IRawResourceChecker>(MockBehavior.Strict).Object;
        // Act
        var exception = Record.Exception(() =>
            _ = new ResourceChecker(rawChecker, null!));
        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}