namespace beetobee.catalog.api.Models;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string SKU { get; set; } = default!;
    public decimal Price { get; set; }
    public int AvailableQuantity { get; set; }
    public Category Category { get; set; } = default!;


}
