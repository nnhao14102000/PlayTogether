using System;

namespace PlayTogether.Core.Dtos.Outcoming.Business.Charity
{
    public class CharityResponse
    {
        public string Id { get; set; }
        
        public string OrganizationName { get; set; }

        public string Avatar { get; set; }

        public string Information { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public float Balance { get; set; }

        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
