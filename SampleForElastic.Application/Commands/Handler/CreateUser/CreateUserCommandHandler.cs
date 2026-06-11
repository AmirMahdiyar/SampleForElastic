using MediatR;
using SampleForElastic.Application.Commands.Contracts;
using SampleForElastic.Application.Contracts;

namespace SampleForElastic.Application.Commands.Handler.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserCommandResponse>
    {
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserCommandHandler(IUserWriteRepository userWriteRepository, IUnitOfWork unitOfWork)
        {
            _userWriteRepository = userWriteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            request.Validate();

            var command = await request.WithCheckingUserExistance(_userWriteRepository, cancellationToken);
            var user = command.ToUser();

            await _userWriteRepository.AddAsync(user, cancellationToken);
            await user.Commit(_unitOfWork, cancellationToken);

            return new CreateUserCommandResponse
            {
                UserId = user.Id
            };
        }
    }
}
