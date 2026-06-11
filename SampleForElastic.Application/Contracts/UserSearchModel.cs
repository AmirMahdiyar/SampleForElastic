using System;
using System.Collections.Generic;

namespace SampleForElastic.Application.Contracts
{
    public class UserSearchModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public DateOnly BirthDate { get; set; }
        public string About { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<CarSearchModel> Cars { get; set; } = new();
    }

    public class CarSearchModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
    }
}
