using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleForElastic.Domain.Enums;
using SampleForElastic.Domain.Models.UserAggregate;

namespace SampleForElastic.Infraustructure.Configurations
{

    namespace SampleForElastic.Infrastructure.Configurations
    {
        public class UserConfiguration : IEntityTypeConfiguration<User>
        {
            public void Configure(EntityTypeBuilder<User> builder)
            {
                builder.ToTable("Users", "dbo");

                builder.HasKey(x => x.Id);

                builder.Property(x => x.Id)
                    .ValueGeneratedNever();

                builder.Property(x => x.BirthDate)
                    .IsRequired()
                    .HasColumnType("date");

                builder.Property(x => x.About)
                    .IsRequired()
                    .HasMaxLength(2000)
                    .HasColumnType("nvarchar(2000)");

                builder.Property(x => x.CreatedAt)
                    .IsRequired()
                    .HasColumnType("datetime2");

                builder.Property(x => x.State)
                    .IsRequired()
                    .HasDefaultValue(ExistanceState.Active)
                    .HasConversion<byte>()
                    .HasColumnType("tinyint");

                builder.HasIndex(x => x.BirthDate)
                    .HasDatabaseName("IX_Users_BirthDate");

                builder.HasMany(x => x.Cars)
                    .WithOne()
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.Ignore(x => x.Events);

            }
        }
    }
}
