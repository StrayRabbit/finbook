using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.AggregatesModel;

namespace Project.Infrastructure.EntityConfigurations
{
    public class ProjectVisibleRuleEntityConfiguration : IEntityTypeConfiguration<ProjectVisibleRule>
    {
        public void Configure(EntityTypeBuilder<ProjectVisibleRule> builder)
        {
            builder
                .ToTable("ProjectVisibleRules")
                .HasKey(p => p.ProjectId);
        }
    }
}
