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
    internal class FluentIngredient : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder.HasKey(i => i.IngredientId);

            builder.Property(i => i.IngredientName)
               .IsRequired()
               .HasMaxLength(255);

            builder.Property(p => p.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}
