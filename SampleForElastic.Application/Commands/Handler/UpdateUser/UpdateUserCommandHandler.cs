using MediatR;
using SampleForElastic.Application.Commands.Contracts;
using SampleForElastic.Application.Contracts;

namespace SampleForElastic.Application.Commands.Handler.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserCommandHandler(IUserWriteRepository userWriteRepository, IUnitOfWork unitOfWork)
        {
            _userWriteRepository = userWriteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            request.Validate();

            var user = await _userWriteRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {request.UserId} was not found.");

            user.UpdateUsername(request.Username);
            user.UpdateAbout(request.About);

            var result = await _unitOfWork.Commit(cancellationToken);
            return result.IsSucceeded;
        }
    }
}
