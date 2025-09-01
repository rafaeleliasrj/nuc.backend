using System;
using System.Collections.Generic;
using Avvo.Domain.Commons;

namespace Avvo.Domain.Entities
{
    /// <summary>
    /// Representa uma variação de um produto (ex: cor, tamanho).
    /// </summary>
    public sealed class ProductVariation : Entity
    {
        public string Type { get; private set; }
        public string Value { get; private set; }

        private ProductVariation() { } // ORM

        public ProductVariation(Guid? id, string type, string value)
        {
            Id = id ?? Guid.NewGuid();
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public void Update(string type, string value)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}
