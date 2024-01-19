using Abstractions.Logic;
using Service.Services;

namespace Service.Tests;

public class ResourceObserverServiceTests
{
    [Fact(DisplayName = $"{nameof(ResourceObserverService)} could be created with valid params")]
    [Trait("Category", "Unit")]
    public void ResourceObserverServiceCouldBeCreatedWithValidParams()
    {
        // Arrange
        var resourceObserver = new Mock<IResourcesObserver>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => { _ = new ResourceObserverService(resourceObserver.Object); });

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"{nameof(ResourceObserverService)} can't be created without resource observer")]
    [Trait("Category", "Unit")]
    public void ResourceObserverServiceCannotBeCreatedWithoutResourceObserver()
    {
        // Arrange
        IResourcesObserver resourceObserver = null!;
        
        // Act
        var exception = Record.Exception(() => { _ = new ResourceObserverService(resourceObserver); });
        
        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}