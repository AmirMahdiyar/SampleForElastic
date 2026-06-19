using MediatR;
using SampleForElastic.Application.Commands.Contracts;
using SampleForElastic.Application.Contracts;
using System.Threading;
using System.Threading.Tasks;

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

            var user = await request.LoadUser(_userWriteRepository, cancellationToken);
            
            return await user
                .UpdateDomain(request)
                .Commit(_unitOfWork, cancellationToken);
        }
    }
}
