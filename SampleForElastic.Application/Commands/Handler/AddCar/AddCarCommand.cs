using MediatR;
using SampleForElastic.Application.Commands.Base;
using SampleForElastic.Application.Commands.Base.Extension;
using System;

namespace SampleForElastic.Application.Commands.Handler.AddCar
{
    public class AddCarCommand : CommandBase, IRequest<Guid>
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;

        public override void Validate()
        {
            new AddCarCommandValidator().Validate(this).ThrowIfNeeded();
        }
    }
}
