using FluentAssertions;

namespace Models.Tests;

public class ResourceNameTests
{
    [Fact(DisplayName = "Constructor initializes Value property with valid name")]
    [Trait("Category", "Unit")]
    public void ConstructorInitializesValuePropertyWithValidName()
    {
        // Arrange
        var nameValue = "ExampleResourceName";

        // Act
        var resourceName = new ResourceName(nameValue);

        // Assert
        resourceName.Value.Should().Be(nameValue);
    }

    [Theory(DisplayName = "Constructor throws ArgumentNullException for null or empty name")]
    [InlineData(null)]
    [InlineData("")]
    [Trait("Category", "Unit")]
    public void ConstructorThrowsArgumentNullExceptionForNullOrEmptyName(string name)
    {
        // Act
        Action act = () => _ = new ResourceName(name);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Constructor throws ArgumentException for whitespace name")]
    [Trait("Category", "Unit")]
    public void ConstructorThrowsArgumentExceptionForWhiteSpaceName()
    {
        // Arrange
        var name = "   ";
        // Act
        Action act = () => _ = new ResourceName(name);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact(DisplayName = "Equals method returns true for equal resource names")]
    [Trait("Category", "Unit")]
    public void EqualsMethodReturnsTrueForEqualResourceNames()
    {
        // Arrange
        var name1 = new ResourceName("TestName");
        var name2 = new ResourceName("TestName");

        // Act and Assert
        name1.Equals(name2).Should().BeTrue();
    }

    [Fact(DisplayName = "Equals method returns false for different resource names")]
    [Trait("Category", "Unit")]
    public void EqualsMethodReturnsFalseForDifferentResourceNames()
    {
        // Arrange
        var name1 = new ResourceName("Name1");
        var name2 = new ResourceName("Name2");

        // Act and Assert
        name1.Equals(name2).Should().BeFalse();
    }

    [Fact(DisplayName = "GetHashCode method returns the same hash code for equal resource names")]
    [Trait("Category", "Unit")]
    public void GetHashCodeMethodReturnsSameHashCodeForEqualResourceNames()
    {
        // Arrange
        var name1 = new ResourceName("TestName");
        var name2 = new ResourceName("TestName");

        // Act and Assert
        name1.GetHashCode().Should().Be(name2.GetHashCode());
    }

    [Fact(DisplayName = "ToString method returns the name value")]
    [Trait("Category", "Unit")]
    public void ToStringMethodReturnsNameValue()
    {
        // Arrange
        var nameValue = "ExampleResourceName";
        var resourceName = new ResourceName(nameValue);

        // Act and Assert
        resourceName.ToString().Should().Be(nameValue);
    }
}
