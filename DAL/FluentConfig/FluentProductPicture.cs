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
    public class FluentProductPicture : IEntityTypeConfiguration<ProductPicture>
    {
        public void Configure(EntityTypeBuilder<ProductPicture> builder)
        {
            builder.HasKey(p => p.PictureId);

            builder.Property(p => p.PictureUrl)
               .IsRequired()
               .HasMaxLength(255);
            
            builder.Property(p => p.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}
