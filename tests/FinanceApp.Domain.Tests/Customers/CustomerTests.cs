using FinanceApp.Domain.Customers;
using FinanceApp.Domain.Customers.Events;
using FluentAssertions;

namespace FinanceApp.Domain.Tests.Customers;

public class CustomerTests
{
    [Fact]
    public void Create_WithValidData_ShouldReturnCustomerWithPendingVerificationStatus()
    {
        // Arrange & Act
        var customer = Customer.Create("Leandro Silva", "leandro@email.com", "529.982.247-25", "11987654321");

        // Assert
        customer.Name.Should().Be("Leandro Silva");
        customer.Email.Value.Should().Be("leandro@email.com");
        customer.Status.Should().Be(CustomerStatus.PendingVerification);
        customer.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Create_ShouldRaiseDomainEvent_CustomerRegistered()
    {
        // Arrange & Act
        var customer = Customer.Create("Leandro Silva", "leandro@email.com", "529.982.247-25", "11987654321");

        // Assert
        customer.DomainEvents.Should().HaveCount(1);
        customer.DomainEvents[0].Should().BeOfType<CustomerRegisteredEvent>();

        var evt = (CustomerRegisteredEvent)customer.DomainEvents[0];
        evt.CustomerId.Should().Be(customer.Id);
        evt.Email.Should().Be("leandro@email.com");
    }

    [Fact]
    public void Verify_WhenPendingVerification_ShouldChangeStatusToVerified()
    {
        // Arrange
        var customer = Customer.Create("Leandro Silva", "leandro@email.com", "529.982.247-25", "11987654321");
        customer.ClearDomainEvents();

        // Act
        customer.Verify();

        // Assert
        customer.Status.Should().Be(CustomerStatus.Verified);
        customer.VerifiedAt.Should().NotBeNull();
        customer.DomainEvents.Should().HaveCount(1);
        customer.DomainEvents[0].Should().BeOfType<CustomerVerifiedEvent>();
    }

    [Fact]
    public void Verify_WhenAlreadyVerified_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var customer = Customer.Create("Leandro Silva", "leandro@email.com", "529.982.247-25", "11987654321");
        customer.Verify();

        // Act & Assert
        var act = () => customer.Verify();
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*already verified*");
    }

    [Theory]
    [InlineData("")]
    [InlineData("invalid-email")]
    [InlineData("no-at-sign")]
    public void Create_WithInvalidEmail_ShouldThrow(string invalidEmail)
    {
        // Arrange & Act & Assert
        var act = () => Customer.Create("Nome", invalidEmail, "529.982.247-25", "11987654321");
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("000.000.000-00")] // all same digits
    [InlineData("111.111.111-11")]
    [InlineData("123.456.789-00")] // invalid check digits
    public void Create_WithInvalidCPF_ShouldThrow(string invalidCpf)
    {
        // Arrange & Act & Assert
        var act = () => Customer.Create("Nome", "valid@email.com", invalidCpf, "11987654321");
        act.Should().Throw<ArgumentException>();
    }
}
