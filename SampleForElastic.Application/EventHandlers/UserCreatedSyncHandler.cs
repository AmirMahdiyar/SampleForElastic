using MediatR;
using SampleForElastic.Application.Contracts;
using SampleForElastic.Application.Exceptions;
using SampleForElastic.Domain.Events.User;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mokeb.Application.EventHandlers
{
    public class UserCreatedSyncHandler : INotificationHandler<UserCreated>
    {
        private readonly IUserReadRepository _readRepository;

        public UserCreatedSyncHandler(IUserReadRepository readRepository)
        {
            _readRepository = readRepository;
        }

        public async Task Handle(UserCreated notification, CancellationToken cancellationToken)
        {
            if (notification.UserId == Guid.Empty || string.IsNullOrWhiteSpace(notification.Username))
            {
                throw new ArgumentException("Invalid user creation event data for synchronization.");
            }

            var model = new UserSearchModel
            {
                Id = notification.UserId,
                Username = notification.Username,
                BirthDate = notification.BirthDate,
                About = notification.About,
                CreatedAt = notification.CreatedAt,
                Cars = new List<CarSearchModel>()
            };

            var success = await _readRepository.IndexAsync(model, cancellationToken);
            if (!success)
            {
                throw new ElasticsearchDocumentIndexingFailedException(notification.UserId, "Failed to index user document.");
            }
        }
    }
}
