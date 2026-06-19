using MediatR;
using SampleForElastic.Application.Contracts;
using SampleForElastic.Application.Exceptions;
using SampleForElastic.Domain.Events.User;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mokeb.Application.EventHandlers
{
    public class UserUpdatedSyncHandler : INotificationHandler<UserUpdated>
    {
        private readonly IUserReadRepository _readRepository;

        public UserUpdatedSyncHandler(IUserReadRepository readRepository)
        {
            _readRepository = readRepository;
        }

        public async Task Handle(UserUpdated notification, CancellationToken cancellationToken)
        {
            if (notification.UserId == Guid.Empty)
            {
                throw new ArgumentException("Invalid user update event data for synchronization.");
            }

            if (notification.State == SampleForElastic.Domain.Enums.ExistanceState.Deleted)
            {
                var success = await _readRepository.DeleteAsync(notification.UserId, cancellationToken);
                if (!success)
                {
                    throw new ElasticsearchDocumentDeletingFailedException(notification.UserId, "Failed to delete user document.");
                }
                return;
            }

            var updateSuccess = await _readRepository.UpdateUserFieldsAsync(
                notification.UserId,
                notification.Username,
                notification.About,
                notification.BirthDate,
                cancellationToken);

            if (!updateSuccess)
            {
                throw new ElasticsearchDocumentUpdatingFailedException(notification.UserId, "Failed to update user document properties.");
            }
        }
    }
}
