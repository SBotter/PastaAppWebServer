using BL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.FluentConfig
{
    internal class FluentProductPackage : IEntityTypeConfiguration<ProductPackage>
    {
        public void Configure(EntityTypeBuilder<ProductPackage> builder)
        {
            builder.HasKey(p => p.PackageId);

            builder.Property(p => p.PackageName)
               .IsRequired()
               .HasMaxLength(255);

            builder.Property(p => p.PackageDescription)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(p => p.PackageUnit)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(p => p.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}

