using Mapster;

namespace beetobee.catalog.api.Features.Product.Queries;

public class GetProductByIdHandler(IProductRepository productRepository, ILogger<GetProductByIdHandler> logger) 
           : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving product with ID {ProductId}", request.Id);

        var product = await productRepository.GetProductByIdAsync(request.Id, cancellationToken);

        logger.LogInformation("Successfully retrieved product with ID {ProductId}", request.Id);
        return new GetProductByIdResult(product.Adapt<ProductDto>());
    }
}
