using FluentAssertions;
using MicrolisR.Mapping.Abstractions;
using MicrolisR.Mapping.Test.Dbo;
using MicrolisR.Mapping.Test.Entities;
using Xunit;

namespace MicrolisR.Mapping.Test;

public class EqualityTests
{
    private static readonly IMapper Sut = new Mapper();
    
    [Fact]
    public void Should_Be_Equal_After_Remapping()
    {
        // Arrange
        var expectedCustomer = new Customer() {Age = 23, Firstname = "Tom", Surname = "Holland"};

        // Act
        var customerDbo = Sut.Map<CustomerDbo>(expectedCustomer);
        var actualCustomer = Sut.Map<Customer>(customerDbo);
        
        // Assert
        actualCustomer.Should().BeEquivalentTo(expectedCustomer);
    }
    
    [Fact]
    public void Should_Be_Equal_After_Mapping()
    {
        // Arrange
        var expectedCustomer = new Customer() {Age = 23, Firstname = "Tom", Surname = "Holland"};

        // Act
        var actualCustomer = Sut.Map<CustomerDbo>(expectedCustomer);

        // Assert
        actualCustomer.Should().BeEquivalentTo(expectedCustomer);
    }
    
    [Fact]
    public void Should_Be_Equal_After_Remapping_With_Different_Primitive_Types()
    {
        // Arrange
        var expectedCustomer = new Customer() {Age = 23, Firstname = "Tom", Surname = "Holland"};

        // Act
        var customerDbo = Sut.Map<CustomerDboWithDifferentPrimitiveTypes>(expectedCustomer);
        var actualCustomer = Sut.Map<Customer>(customerDbo);

        // Assert
        actualCustomer.Should().BeEquivalentTo(expectedCustomer);
    }
    
    [Fact]
    public void Should_Be_Equal_After_Remapping_With_Nullable_Types()
    {
        // Arrange
        var expectedCustomer = new Customer() {Age = 23, Firstname = "Tom", Surname = "Holland"};

        // Act
        var customerDbo = Sut.Map<CustomerDboWithNullableTypes>(expectedCustomer);
        var actualCustomer = Sut.Map<Customer>(customerDbo);

        // Assert
        actualCustomer.Should().BeEquivalentTo(expectedCustomer);
    }
    
    [Fact]
    public void Should_Be_Equal_After_Mapping_With_Nullable_Types()
    {
        // Arrange
        var expectedCustomer = new Customer() {Age = 23, Firstname = "Tom", Surname = "Holland"};

        // Act
        var actualCustomer = Sut.Map<CustomerDboWithNullableTypes>(expectedCustomer);

        // Assert
        actualCustomer.Should().BeEquivalentTo(expectedCustomer);
    }
}