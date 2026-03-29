namespace beetobee.catalog.api.Features.Product.Queries;

public record GetProductsQuery(PaginationRequest PaginationRequest) : IQuery<GetProductsResult>;
public record GetProductsResult(PaginatedResult<ProductDto> Products);

