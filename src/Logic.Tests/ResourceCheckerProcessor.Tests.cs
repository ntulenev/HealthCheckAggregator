using Abstractions.Logic;
using Models;

using Microsoft.Extensions.Logging;

namespace Logic.Tests;

public class ResourceCheckerProcessorTests
{
    [Fact(DisplayName = $"{nameof(ResourceCheckerProcessor)} can be created with valid params")]
    [Trait("Category", "Unit")]
    public void ResourceCheckerProcessorCanBeCreatedWithValidParams()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<ResourceCheckerProcessor>>();
        var resourceChecker = new Mock<IResourceChecker>(MockBehavior.Strict);
        var resourceHealthCheck = new Mock<ResourceHealthCheck>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() =>
            _ = new ResourceCheckerProcessor(loggerMock.Object, resourceChecker.Object,
                resourceHealthCheck.Object));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"{nameof(ResourceCheckerProcessor)} cant be created without logger")]
    [Trait("Category", "Unit")]
    public void ResourceCheckerProcessorCannotBeCreatedWithoutLogger()
    {
        // Arrange
        var resourceChecker = new Mock<IResourceChecker>(MockBehavior.Strict);
        var resourceHealthCheck = new Mock<ResourceHealthCheck>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() =>
            _ = new ResourceCheckerProcessor(null!, resourceChecker.Object,
                resourceHealthCheck.Object));
        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(ResourceCheckerProcessor)} cant be created without resource checker")]
    [Trait("Category", "Unit")]
    public void ResourceCheckerProcessorCannotBeCreatedWithoutResourceChecker()
    {
        // Arrange
        var logger = new Mock<ILogger<ResourceCheckerProcessor>>();
        var resourceHealthCheck = new Mock<ResourceHealthCheck>(MockBehavior.Strict);
        // Act
        var exception = Record.Exception(() =>
            _ = new ResourceCheckerProcessor(logger.Object, null!,
                resourceHealthCheck.Object));
        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(ResourceCheckerProcessor)} cant be created without healthcheck")]
    [Trait("Category", "Unit")]
    public void ResourceCheckerProcessorCannotBeCreatedWithoutHealthCheck()
    {
        // Arrange
        var logger = new Mock<ILogger<ResourceCheckerProcessor>>();
        var resourceChecker = new Mock<IResourceChecker>(MockBehavior.Strict);
        // Act
        var exception = Record.Exception(() =>
            _ = new ResourceCheckerProcessor(logger.Object, resourceChecker.Object,
                null!));
        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}