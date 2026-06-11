using SampleForElastic.Domain.Base;
using SampleForElastic.Domain.Enums;

namespace SampleForElastic.Domain.Models.Outbox
{
    public class OutboxMessage
    {
        public int Id { get; set; }

        public Guid EventId { get; set; }
        public required string EventType { get; set; } = typeof(EventBase).Name;
        public required string Payload { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? ProcessedAt { get; set; }
        public OutboxStatus Status { get; set; }
        public OutboxMessage() { }

        public OutboxMessage(Guid eventId, string eventType, string payload)
        {
            EventId = eventId;
            EventType = eventType;
            Payload = payload;
            CreatedAt = DateTime.UtcNow;
            Status = OutboxStatus.Pending;
        }

        public void MarkAsProcessed()
        {
            Status = OutboxStatus.Processed;
            ProcessedAt = DateTime.Now;
        }

        public void MarkAsFailed()
        {
            Status = OutboxStatus.Failed;
        }

        public void ResetToPending()
        {
            Status = OutboxStatus.Pending;
            ProcessedAt = null;
        }
    }
}
