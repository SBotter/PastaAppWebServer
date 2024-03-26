using BL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.FluentConfig
{
    public class FluentProduct: IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.ProductId);

            builder.Property(p => p.ProductName)
               .IsRequired()
               .HasMaxLength(255);

            builder.Property(p => p.ProductDescription)
                .IsRequired()
                .HasMaxLength(2000);

            builder.HasOne<Company>()
                .WithMany()
                .HasForeignKey(p => p.CompanyId);

            builder.Property(p => p.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}
