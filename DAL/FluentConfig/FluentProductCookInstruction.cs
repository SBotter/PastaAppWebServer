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
    internal class FluentProductCookInstruction : IEntityTypeConfiguration<ProductCookInstruction>
    {
        public void Configure(EntityTypeBuilder<ProductCookInstruction> builder)
        {
            builder.HasKey(p => new { p.ProductId, p.CookInstructionId });
        }
    }
}
