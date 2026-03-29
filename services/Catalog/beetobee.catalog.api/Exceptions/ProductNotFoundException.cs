using System;
using commonblock.Exceptions;

namespace beetobee.catalog.api.Exceptions;

public class ProductNotFoundException : NotFoundException
{
    public ProductNotFoundException(Guid productId)
        : base($"Product with ID {productId} not found.")
    {
    }
}
