namespace Wrecept.Domain;

public sealed class Customer
{
    public Guid Id { get; }
    public string Name { get; }

    public Customer(Guid id, string name)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Customer id cannot be empty.", nameof(id));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Customer name cannot be empty.", nameof(name));

        Id = id;
        Name = name;
    }
}
