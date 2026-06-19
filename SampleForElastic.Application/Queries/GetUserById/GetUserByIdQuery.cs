using MediatR;
using Mokeb.Application.Queries.GetUserById;
using System;

namespace SampleForElastic.Application.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<GetUserByIdQueryResponse>
    {
        public Guid UserId { get; set; }

        public GetUserByIdQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
