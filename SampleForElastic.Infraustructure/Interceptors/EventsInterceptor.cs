using Microsoft.EntityFrameworkCore.Diagnostics;
using SampleForElastic.Domain.Base;
using SampleForElastic.Domain.Models.Outbox;
using System.Text.Json;

namespace SampleForElastic.Infraustructure.Interceptors
{
    public class EventsInterceptor : SaveChangesInterceptor
    {
        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;

            var outboxMessages = context!
                .ChangeTracker
                .Entries<DomainEventBase<EventBase>>()
                .Select(x => x.Entity)
                .ToList()
                .SelectMany(x =>
                {
                    var domainEvents = x.Events.ToList();
                    x.ClearEvents();
                    return domainEvents;
                })
                .Select(x => new OutboxMessage()
                {
                    EventId = Guid.NewGuid(),
                    EventType = x.GetType().AssemblyQualifiedName ?? x.GetType().Name,
                    Payload = JsonSerializer.Serialize(x, x.GetType()),
                    Status = Domain.Enums.OutboxStatus.Pending,
                })
                .ToList();

            if (outboxMessages.Any())
            {
                context.Set<OutboxMessage>().AddRange(outboxMessages);
            }

            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
