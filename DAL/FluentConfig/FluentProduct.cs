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

            builder.Property(p => p.ProductType)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasOne<Company>()
                .WithMany()
                .HasForeignKey(p => p.CompanyId);

            builder.Property(p => p.IsDeleted)
                .HasDefaultValue(false);

            builder.HasMany(p => p.Categories)
                .WithMany(c => c.products)
                .UsingEntity<ProductCategory>(
                    j => j
                        .HasOne(pc => pc.Category)
                        .WithMany()
                        .HasForeignKey(pc => pc.CategoryId),
                    j => j
                        .HasOne(pc => pc.Product)
                        .WithMany()
                        .HasForeignKey(pc => pc.ProductId),
                    j =>
                    {
                        j.HasKey(pc => new { pc.ProductId, pc.CategoryId });
                        j.ToTable("ProductCategories");
                    });

            builder.HasMany(p => p.ProductIngredients)
                .WithMany(i => i.products)
                .UsingEntity<ProductIngredient>(
                    j => j
                        .HasOne(ping => ping.Ingredient)
                        .WithMany()
                        .HasForeignKey(ping => ping.IngredientId),
                    j => j
                        .HasOne(ping => ping.Product)
                        .WithMany()
                        .HasForeignKey(ping => ping.ProductId),
                    j =>
                    {
                        j.HasKey(ping => new { ping.ProductId, ping.IngredientId });
                        j.ToTable("ProductIngredients");
                    });

            builder.HasMany(p => p.ProductCookInstructions)
                .WithMany(ci => ci.products)
                .UsingEntity<ProductCookInstruction>(
                    j => j
                        .HasOne(ci => ci.CookInstruction)
                        .WithMany()
                        .HasForeignKey(ci => ci.CookInstructionId),
                    j => j
                        .HasOne(ci => ci.Product)
                        .WithMany()
                        .HasForeignKey(ci => ci.ProductId),
                    j =>
                    {
                        j.HasKey(ci => new { ci.ProductId, ci.CookInstructionId });
                        j.ToTable("ProductCookInstructions");
                    });
        }
    }
}
