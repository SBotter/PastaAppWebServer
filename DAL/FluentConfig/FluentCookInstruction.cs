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
    internal class FluentCookInstruction : IEntityTypeConfiguration<CookInstruction>
    {
        public void Configure(EntityTypeBuilder<CookInstruction> builder)
        {
            builder.HasKey(ci => ci.CookInstructionId);

            builder.Property(ci => ci.CookInstructionDescription)
               .IsRequired()
               .HasMaxLength(255);

            builder.Property(p => p.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}
