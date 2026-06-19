using MediatR;
using SampleForElastic.Application.Commands.Contracts;
using SampleForElastic.Application.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

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

            var user = await request.LoadUser(_userWriteRepository, cancellationToken);
            var car = request.ToCar();

            await user
                .AddCarToAggregate(car)
                .Commit(_unitOfWork, cancellationToken);

            return car.Id;
        }
    }
}
