using Refit;

namespace beetobee.order.application.Services;

public interface IProductService
{
    [Get("/api/products/{productId}")]
    Task<ProductResponseDto> GetProductByIdAsync(string productId, CancellationToken cancellationToken = default);
}
