using System;

namespace PlayTogether.Core.Dtos.Outcoming.Business.Hirer
{
    public class HirerGetByIdForAdminResponse
    {
        public string Id { get; set; }

        public string Email { get; set; }
        
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string City { get; set; }

        public DateTime DateOfBirth { get; set; }

        public bool Gender { get; set; }
    }
}