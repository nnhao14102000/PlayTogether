using System;

namespace PlayTogether.Core.Dtos.Incoming.Auth
{
    public class RegisterUserInfoRequest: RegisterRequest
    { 
        public string Firstname { get; set; }

        public string  Lastname { get; set; }

        public string City { get; set; }

        public DateTime DateOfBirth { get; set; }

        public bool Gender { get; set; }

        public bool ConfirmEmail { get; set; }
    }
}
