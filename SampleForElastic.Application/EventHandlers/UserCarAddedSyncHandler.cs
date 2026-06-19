using MediatR;
using SampleForElastic.Application.Contracts;
using SampleForElastic.Application.Exceptions;
using SampleForElastic.Domain.Events.User;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mokeb.Application.EventHandlers
{
    public class UserCarAddedSyncHandler : INotificationHandler<UserCarAdded>
    {
        private readonly IUserReadRepository _readRepository;

        public UserCarAddedSyncHandler(IUserReadRepository readRepository)
        {
            _readRepository = readRepository;
        }

        public async Task Handle(UserCarAdded notification, CancellationToken cancellationToken)
        {
            if (notification.UserId == Guid.Empty || notification.CarId == Guid.Empty || string.IsNullOrWhiteSpace(notification.Name))
            {
                throw new ArgumentException("Invalid car addition event data for synchronization.");
            }

            var carModel = new CarSearchModel
            {
                Id = notification.CarId,
                Name = notification.Name,
                Code = notification.Code,
                Color = notification.Color
            };

            var success = await _readRepository.AddCarAsync(notification.UserId, carModel, cancellationToken);
            if (!success)
            {
                throw new ElasticsearchDocumentUpdatingFailedException(notification.UserId, "Failed to add car to user document.");
            }
        }
    }
}
