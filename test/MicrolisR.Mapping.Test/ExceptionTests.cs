using System;
using FluentAssertions;
using MicrolisR.Mapping.Abstractions;
using MicrolisR.Mapping.Test.Entities;
using Xunit;

namespace MicrolisR.Mapping.Test;

public class ExceptionTests
{
    private static readonly IMapper Sut = new Mapper();
    
    [Fact]
    public void Should_Not_Throws_NullReferenceException_After_Mapping_With_Nullable_Types()
    {
        // Arrange
        var expectedCustomer = new Customer() {Age = 23, Firstname = "Tom", Surname = "Holland"};

        // Act
        var resultFunc = () => Sut.Map<object>(expectedCustomer);
        
        // Assert
        resultFunc.Should().NotThrow<NullReferenceException>();
    }
    
    
    
    
    
}