using System;
namespace PlayTogether.Core.Dtos.Incoming.Business.Player
{
    public class OtherSkillUpdateRequest
    {
        public string OtherSkill { get; set; }
        public DateTime UpdateDate = DateTime.UtcNow.AddHours(7);
    }
}