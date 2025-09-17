using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Avvo.Domain.Entities;

namespace Avvo.Domain.Mappings
{
    public partial class BusinessMap : IEntityTypeConfiguration<Business>
    {
        public virtual void Configure(EntityTypeBuilder<Business> builder)
        {

        }
    }
}
