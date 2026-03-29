namespace beetobee.order.application.Dtos;

public class ProductResponseDto
{
    public Product product { get; set; } = default!;
}


public class Product
{
    public string id { get; set; } = default!;
    public string name { get; set; } = default!;
    public string description { get; set; } = default!;
    public string sku { get; set; } = default!;
    public double price { get; set; }
    public int availableQuantity { get; set; }
    public int category { get; set; }
}

