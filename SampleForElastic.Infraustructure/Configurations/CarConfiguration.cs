using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleForElastic.Domain.Enums;
using SampleForElastic.Domain.Models.UserAggregate;

namespace SampleForElastic.Infraustructure.Configurations
{

    namespace SampleForElastic.Infrastructure.Configurations
    {
        public class CarConfiguration : IEntityTypeConfiguration<Car>
        {
            public void Configure(EntityTypeBuilder<Car> builder)
            {
                builder.ToTable("Cars", "dbo");

                builder.HasKey(x => x.Id);

                builder.Property(x => x.Id)
                    .ValueGeneratedNever();

                builder.Property(x => x.UserId)
                    .IsRequired();

                builder.Property(x => x.PostedAt)
                    .IsRequired()
                    .HasColumnType("datetime2");

                builder.Property(x => x.State)
                    .IsRequired()
                    .HasDefaultValue(ExistanceState.Active)
                    .HasConversion<byte>()
                    .HasColumnType("tinyint");

                builder.OwnsOne(x => x.Identity, identity =>
                {
                    identity.Property(p => p.Name)
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnName("CarName")
                        .HasColumnType("nvarchar(100)");

                    identity.Property(p => p.Code)
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnName("CarCode")
                        .HasColumnType("nvarchar(50)");

                    identity.Property(p => p.Color)
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnName("CarColor")
                        .HasColumnType("nvarchar(50)");

                    identity.HasIndex(p => p.Code)
                        .HasDatabaseName("IX_Cars_Code");

                    identity.HasIndex(p => new { p.Code, p.Name })
                        .HasDatabaseName("IX_Cars_Code_Name");
                });

                builder.HasIndex(x => x.UserId)
                    .HasDatabaseName("IX_Cars_UserId");

                builder.HasIndex(x => x.PostedAt)
                    .HasDatabaseName("IX_Cars_PostedAt");

                builder.Ignore(x => x.Events);
            }
        }
    }
}
