namespace beetobee.catalog.api.Features.Product.Queries;

public class GetProductsHandler(IProductRepository productRepository, ILogger<GetProductsHandler> logger) 
           : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var pageIndex = request.PaginationRequest.PageIndex;
        var pageSize = request.PaginationRequest.PageSize;

        logger.LogInformation("Fetching products for page {PageIndex} with size {PageSize}", pageIndex, pageSize);

        var allProducts = (await productRepository.GetAllProductsAsync(cancellationToken)).ToList();
        var totalCount = allProducts.Count;

        var pagedProducts = allProducts
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                SKU = product.SKU,
                Price = product.Price,
                AvailableQuantity = product.AvailableQuantity,
                Category = product.Category
            })
            .ToList();

        logger.LogInformation(
            "Returning {ReturnedCount} products out of {TotalCount}",
            pagedProducts.Count,
            totalCount);

        return new GetProductsResult(
            new PaginatedResult<ProductDto>(pageIndex, pageSize, totalCount, pagedProducts));
    }
}
