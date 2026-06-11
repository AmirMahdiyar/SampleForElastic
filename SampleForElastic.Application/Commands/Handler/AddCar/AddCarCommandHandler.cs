using MediatR;
using SampleForElastic.Application.Commands.Contracts;
using SampleForElastic.Application.Contracts;
using SampleForElastic.Domain.Models.UserAggregate;
using SampleForElastic.Domain.Models.ValueObjects;

namespace SampleForElastic.Application.Commands.Handler.AddCar
{
    public class AddCarCommandHandler : IRequestHandler<AddCarCommand, Guid>
    {
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddCarCommandHandler(IUserWriteRepository userWriteRepository, IUnitOfWork unitOfWork)
        {
            _userWriteRepository = userWriteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(AddCarCommand request, CancellationToken cancellationToken)
        {
            request.Validate();

            var user = await _userWriteRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {request.UserId} was not found.");

            var carIdentity = CarIdentity.Create(request.Name, request.Code, request.Color);
            var car = new Car(carIdentity, request.UserId);

            user.AddCar(car);

            await _unitOfWork.Commit(cancellationToken);

            return car.Id;
        }
    }
}
