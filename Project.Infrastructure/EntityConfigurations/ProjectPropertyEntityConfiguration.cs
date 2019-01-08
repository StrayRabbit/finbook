using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.AggregatesModel;

namespace Project.Infrastructure.EntityConfigurations
{
    public class ProjectPropertyEntityConfiguration : IEntityTypeConfiguration<Domain.AggregatesModel.ProjectProperty>
    {
        public void Configure(EntityTypeBuilder<ProjectProperty> builder)
        {
            builder
                .ToTable("ProjectProperties")
                .HasKey(p => new
                {
                    p.ProjectId,
                    p.Key,
                    p.Value
                });

            builder.Property(p => p.Key).HasMaxLength(100);
            builder.Property(p => p.Value).HasMaxLength(100);
        }
    }
}
