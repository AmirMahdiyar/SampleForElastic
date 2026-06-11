using MediatR;
using SampleForElastic.Application.Commands.Base;

namespace SampleForElastic.Application.Commands.Handler.AddCar
{
    public class AddCarCommand : CommandBase, IRequest<Guid>
    {
        public Guid UserId { get; set; }
        public required string Name { get; set; }
        public required string Code { get; set; }
        public required string Color { get; set; }

        public override void Validate()
        {
            if (UserId == Guid.Empty)
                throw new ArgumentException("UserId is required.");
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentException("Car Name is required.");
            if (string.IsNullOrWhiteSpace(Code))
                throw new ArgumentException("Car Code is required.");
            if (string.IsNullOrWhiteSpace(Color))
                throw new ArgumentException("Car Color is required.");
        }
    }
}
