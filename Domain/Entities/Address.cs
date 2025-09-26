namespace Domain.Entities;

public sealed class Address
{
    public int Id { get; set; }
    public string CustomerId { get; set; } = string.Empty;
    public string Governorate { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string? Note { get; set; }
    public bool IsDefault { get; set; }

    public override string ToString() 
        => $"{Governorate}, {City}, {Street}." + 
        (
            (string.IsNullOrEmpty(Note))
            ? string.Empty
            : string.IsNullOrWhiteSpace(Note)
            ? string.Empty
            : $" Note: {Note}"
        );
}
