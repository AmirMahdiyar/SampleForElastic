using MediatR;
using SampleForElastic.Application.Contracts;
using System.Text.Json;

namespace SampleForElastic.BackgroundServices
{
    public class OutboxProcessorBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public OutboxProcessorBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessMessagesAsync(stoppingToken);
                }
                catch
                {
                }
                await Task.Delay(5000, stoppingToken);
            }
        }

        private async Task ProcessMessagesAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var outboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var messages = await outboxRepository.FetchPendingMessagesWithLockAsync(20, cancellationToken);
            if (messages.Count == 0)
            {
                await outboxRepository.SaveChangesAsync(cancellationToken);
                return;
            }

            foreach (var message in messages)
            {
                try
                {
                    var eventType = Type.GetType(message.EventType);
                    if (eventType == null)
                    {
                        message.MarkAsFailed();
                        continue;
                    }

                    var @event = JsonSerializer.Deserialize(message.Payload, eventType);
                    if (@event is not INotification notification)
                    {
                        message.MarkAsFailed();
                        continue;
                    }

                    await mediator.Publish(notification, cancellationToken);
                    message.MarkAsProcessed();
                }
                catch
                {
                    message.MarkAsFailed();
                }
            }

            await outboxRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
