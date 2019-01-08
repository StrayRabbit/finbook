using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Project.Infrastructure.EntityConfigurations
{
    public class ProjectEntityConfiguration : IEntityTypeConfiguration<Domain.AggregatesModel.Project>
    {
        public void Configure(EntityTypeBuilder<Domain.AggregatesModel.Project> builder)
        {
            builder
                .ToTable("Projects")
                .HasKey(p => p.Id);
        }
    }
}
