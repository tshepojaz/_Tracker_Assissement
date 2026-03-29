using beetobee.catalog.api.Models;

namespace beetobee.catalog.api.Data;

public interface IProductRepository
{
   Task <IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken = default);
   Task<Product> GetProductByIdAsync(Guid id, CancellationToken cancellationToken = default);
   Task<Product> AddProductAsync(Product product, CancellationToken cancellationToken = default);
   Task<Product> UpdateProductAsync(Product product, CancellationToken cancellationToken = default);
   Task DeleteProductAsync(Guid id, CancellationToken cancellationToken = default);
}
