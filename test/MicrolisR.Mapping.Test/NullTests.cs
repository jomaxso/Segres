using System;
using FluentAssertions;
using MicrolisR.Mapping.Test.Entities;
using Xunit;

namespace MicrolisR.Mapping.Test;

public class NullTests
{
    private static readonly IMapper Sut = new Mapper();
    
    [Fact]
    public void Should_Throws_NullReferenceException_After_Mapping_With_Nullable_Types()
    {
        // Arrange
        var expectedCustomer = new Customer() {Age = 23, Firstname = "Tom", Surname = "Holland"};

        // Act
        Func<object> resultFunc = () => Sut.Map<object>(expectedCustomer);
        
        // Assert
        resultFunc.Should().Throw<NullReferenceException>();
    }
    
    [Fact]
    public void Should_Be_Null_After_TryMapping_With_Nullable_Types()
    {
        // Arrange
        var expectedCustomer = new Customer() {Age = 23, Firstname = "Tom", Surname = "Holland"};

        // Act
        var actualCustomer = Sut.TryMap<object>(expectedCustomer);
        
        // Assert
        actualCustomer.Should().BeNull();
    }
}