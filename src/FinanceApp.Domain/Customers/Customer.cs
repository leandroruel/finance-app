using FinanceApp.Domain.Common;
using FinanceApp.Domain.Customers.Events;
using FinanceApp.Domain.Customers.ValueObjects;

namespace FinanceApp.Domain.Customers;

public sealed class Customer : AggregateRoot
{
    public string Name { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public CPF CPF { get; private set; } = null!;
    public PhoneNumber PhoneNumber { get; private set; } = null!;
    public CustomerStatus Status { get; private set; }
    public DateTime? VerifiedAt { get; private set; }

    private Customer() { }

    private Customer(Guid id, string name, Email email, CPF cpf, PhoneNumber phoneNumber)
        : base(id)
    {
        Name = name;
        Email = email;
        CPF = cpf;
        PhoneNumber = phoneNumber;
        Status = CustomerStatus.PendingVerification;
    }

    public static Customer Create(string name, string email, string cpf, string phoneNumber)
    {
        var customer = new Customer(
            Guid.NewGuid(),
            name,
            Email.Create(email),
            CPF.Create(cpf),
            PhoneNumber.Create(phoneNumber));

        customer.RaiseDomainEvent(new CustomerRegisteredEvent(customer.Id, name, email));
        return customer;
    }

    public void Verify()
    {
        if (Status == CustomerStatus.Verified)
            throw new InvalidOperationException("Customer is already verified.");

        Status = CustomerStatus.Verified;
        VerifiedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new CustomerVerifiedEvent(Id));
    }

    public void Suspend()
    {
        Status = CustomerStatus.Suspended;
        UpdatedAt = DateTime.UtcNow;
    }
}

public enum CustomerStatus
{
    PendingVerification,
    Verified,
    Suspended
}
