using MediatR;
using SampleForElastic.Application.Commands.Base;

namespace SampleForElastic.Application.Commands.Handler.UpdateUser
{
    public class UpdateUserCommand : CommandBase, IRequest<bool>
    {
        public Guid UserId { get; set; }
        public required string Username { get; set; }
        public required string About { get; set; }

        public override void Validate()
        {
            if (UserId == Guid.Empty)
                throw new ArgumentException("UserId is required.");
            if (string.IsNullOrWhiteSpace(Username))
                throw new ArgumentException("Username is required.");
            if (string.IsNullOrWhiteSpace(About))
                throw new ArgumentException("About information is required.");
        }
    }
}
