using System;

namespace PlayTogether.Core.Dtos.Outcoming.Generic
{
    public abstract class BaseUserResponse
    {
        public string Id { get; set; }
        public DateTime? AddedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
