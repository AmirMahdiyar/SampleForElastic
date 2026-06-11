using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SampleForElastic.Domain.Base;
using SampleForElastic.Domain.Models.Outbox;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SampleForElastic.Infraustructure.BackgroundServices
{
    public class OutboxProcessorBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OutboxProcessorBackgroundService> _logger;
        private readonly TimeSpan _pollingInterval = TimeSpan.FromSeconds(5);

        public OutboxProcessorBackgroundService(IServiceProvider serviceProvider, ILogger<OutboxProcessorBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Outbox Processor Background Service is starting...");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessOutboxMessagesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while processing outbox messages.");
                }

                await Task.Delay(_pollingInterval, stoppingToken);
            }

            _logger.LogInformation("Outbox Processor Background Service is stopping.");
        }

        private async Task ProcessOutboxMessagesAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SampleDbContext>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            // CHOICE: Use explicit transaction with pessimistic locking (UPDLOCK, READPAST) on SQL Server.
            // WHY: In a horizontal scaled API setup, multiple replicas of this service might run concurrently.
            // UPDLOCK locks the rows read, preventing other processes from modifying them.
            // READPAST skips already locked rows, preventing blocking and allowing concurrent instances to process subsequent messages safely.
            using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

            var messages = await dbContext.OutboxMessages
                .FromSqlRaw("SELECT TOP (20) * FROM dbo.OutboxMessages WITH (UPDLOCK, READPAST) WHERE Status = 0 ORDER BY Id")
                .ToListAsync(cancellationToken);

            if (messages.Count == 0)
            {
                await transaction.CommitAsync(cancellationToken);
                return;
            }

            _logger.LogInformation("Fetched {Count} pending outbox messages to process.", messages.Count);

            foreach (var message in messages)
            {
                try
                {
                    // CHOICE: Get the type of event using the stored AssemblyQualifiedName.
                    // WHY: This allows precise type resolution for deserialization without manually mapping string names to concrete types.
                    var eventType = Type.GetType(message.EventType);
                    if (eventType == null)
                    {
                        throw new InvalidOperationException($"Could not resolve type '{message.EventType}' for outbox message {message.Id}.");
                    }

                    var @event = JsonSerializer.Deserialize(message.Payload, eventType);
                    if (@event == null)
                    {
                        throw new InvalidOperationException($"Failed to deserialize payload for outbox message {message.Id}.");
                    }

                    if (@event is not INotification notification)
                    {
                        throw new InvalidOperationException($"Event type '{message.EventType}' does not implement INotification.");
                    }

                    // CHOICE: Publish the deserialized event locally through MediatR.
                    // WHY: Decouples the background polling loop from the actual read-side syncing event handlers (SyncElasticsearchEventHandlers).
                    await mediator.Publish(notification, cancellationToken);

                    message.MarkAsProcessed();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to process outbox message {MessageId} (Event: {EventType}). Marking as Failed.", message.Id, message.EventType);
                    message.MarkAsFailed();
                }
            }

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
    }
}
