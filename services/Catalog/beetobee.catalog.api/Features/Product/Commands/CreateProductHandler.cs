using ProductModel = beetobee.catalog.api.Models.Product;

namespace beetobee.catalog.api.Features.Product.Commands;

public class CreateProductHandler (IProductRepository productRepository, ILogger<CreateProductHandler> logger) 
           : ICommandHandler<CreateProductCommand, ProductResult>
{
    public async Task<ProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating product with SKU {Sku}", request.Product.SKU);

        var newProduct = new ProductModel
        {
            Id = Guid.NewGuid(),
            Name = request.Product.Name,
            Description = request.Product.Description,
            SKU = request.Product.SKU,
            Price = request.Product.Price,
            AvailableQuantity = request.Product.AvailableQuantity,
            Category = request.Product.Category
        };

        var createdProduct = await productRepository.AddProductAsync(newProduct, cancellationToken);

        logger.LogInformation("Successfully created product {ProductId}", createdProduct.Id);
        return new ProductResult(createdProduct.Id);
    }

}
