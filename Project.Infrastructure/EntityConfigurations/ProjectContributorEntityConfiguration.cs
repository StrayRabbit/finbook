using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.AggregatesModel;

namespace Project.Infrastructure.EntityConfigurations
{
    public class ProjectContributorEntityConfiguration : IEntityTypeConfiguration<Domain.AggregatesModel.ProjectContributor>
    {
        public void Configure(EntityTypeBuilder<ProjectContributor> builder)
        {
            builder
                .ToTable("ProjectContributors")
                .HasKey(p => p.Id);
        }
    }
}
