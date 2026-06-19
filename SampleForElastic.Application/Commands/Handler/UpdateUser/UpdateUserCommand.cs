using MediatR;
using SampleForElastic.Application.Commands.Base;
using SampleForElastic.Application.Commands.Base.Extension;
using System;

namespace SampleForElastic.Application.Commands.Handler.UpdateUser
{
    public class UpdateUserCommand : CommandBase, IRequest<bool>
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string About { get; set; } = string.Empty;

        public override void Validate()
        {
            new UpdateUserCommandValidator().Validate(this).ThrowIfNeeded();
        }
    }
}
