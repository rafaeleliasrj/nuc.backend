using System;
using System.Collections.Generic;
using System.Linq;
using Avvo.Core.Abstractions;

namespace Avvo.Domain.Entities
{
    public record SaleItem(Guid ProductSkuId, string ProductName, IReadOnlyCollection<ProductVariation> Variations, int Quantity, decimal UnitPrice, decimal Discount)
    {
        public decimal Total => (Quantity * UnitPrice) - Discount;
    }
}
