using PlayTogether.Core.Dtos.Outcoming.Generic;

namespace PlayTogether.Core.Dtos.Outcoming.Business.Admin
{
    public class AdminResponse : BaseUserResponse
    {
        public string IdentityId { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }
    }
}
