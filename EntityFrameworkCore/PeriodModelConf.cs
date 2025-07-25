﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pontocanhoto.Domain;

namespace Pontocanhoto.EntityFrameworkCore
{
    public class PeriodModelConf : IEntityTypeConfiguration<PeriodModel>
    {
        public void Configure(EntityTypeBuilder<PeriodModel> builder)
        {
            builder.ToTable("Period");

            builder.HasKey(period => period.Id);

            builder.Property(period => period.Id)
                .ValueGeneratedOnAdd();

            builder.Property(period => period.StartDate)
                .IsRequired();

            builder.Property(period => period.EndDate)
                .IsRequired();
        }
    }
}
