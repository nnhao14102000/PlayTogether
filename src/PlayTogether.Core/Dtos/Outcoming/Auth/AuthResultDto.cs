using System;
using System.Collections.Generic;

namespace PlayTogether.Core.Dtos.Outgoing.Auth
{
    public class AuthResultDto
    {
        public string Message { get; set; }

        public bool Success { get; set; }

        public List<string> Errors { get; set; }

        public DateTime ExpireDate { get; set; }
    }
}