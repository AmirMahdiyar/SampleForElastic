using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleForElastic.Domain.Enums;
using SampleForElastic.Domain.Models.Outbox;

namespace SampleForElastic.Infraustructure.Configurations
{
    public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.ToTable("OutboxMessages", "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

            builder.Property(x => x.EventId)
                .IsRequired()
                .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.EventType)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.Payload)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime2");

            builder.Property(x => x.ProcessedAt)
                .IsRequired(false)
                .HasColumnType("datetime2");

            builder.Property(x => x.Status)
                .IsRequired()
                .HasDefaultValue(OutboxStatus.Pending)
                .HasConversion<byte>()
                .HasColumnType("tinyint");

            builder.HasIndex(x => new { x.Status, x.CreatedAt })
                .HasDatabaseName("IX_OutboxMessages_Status_CreatedAt");

            builder.HasIndex(x => x.ProcessedAt)
                .HasDatabaseName("IX_OutboxMessages_ProcessedAt");
        }
    }
}
