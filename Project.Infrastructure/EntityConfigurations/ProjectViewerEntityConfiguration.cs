using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.AggregatesModel;

namespace Project.Infrastructure.EntityConfigurations
{
    public class ProjectViewerEntityConfiguration : IEntityTypeConfiguration<Domain.AggregatesModel.ProjectViewer>
    {
        public void Configure(EntityTypeBuilder<ProjectViewer> builder)
        {
            builder
                .ToTable("ProjectViewers")
                .HasKey(p => p.Id);
        }
    }
}
