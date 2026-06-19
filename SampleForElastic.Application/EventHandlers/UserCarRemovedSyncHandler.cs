using MediatR;
using SampleForElastic.Application.Contracts;
using SampleForElastic.Application.Exceptions;
using SampleForElastic.Domain.Events.User;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mokeb.Application.EventHandlers
{
    public class UserCarRemovedSyncHandler : INotificationHandler<UserCarRemoved>
    {
        private readonly IUserReadRepository _readRepository;

        public UserCarRemovedSyncHandler(IUserReadRepository readRepository)
        {
            _readRepository = readRepository;
        }

        public async Task Handle(UserCarRemoved notification, CancellationToken cancellationToken)
        {
            if (notification.UserId == Guid.Empty || notification.CarId == Guid.Empty)
            {
                throw new ArgumentException("Invalid car removal event data for synchronization.");
            }

            var success = await _readRepository.RemoveCarAsync(notification.UserId, notification.CarId, cancellationToken);
            if (!success)
            {
                throw new ElasticsearchDocumentUpdatingFailedException(notification.UserId, "Failed to remove car from user document.");
            }
        }
    }
}
