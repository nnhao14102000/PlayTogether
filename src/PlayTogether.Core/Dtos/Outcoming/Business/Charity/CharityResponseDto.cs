using PlayTogether.Core.Dtos.Outcoming.Generic;

namespace PlayTogether.Core.Dtos.Outcoming.Business.Charity
{
    public class CharityResponseDto : BaseUserResponseDto
    {
        public string IdentityId { get; set; }

        public string OrganizationName { get; set; }

        public string Avatar { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public float Balance { get; set; }

        public bool IsActive { get; set; }
    }
}
