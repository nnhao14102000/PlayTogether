namespace PlayTogether.Core.Dtos.Outcoming.Business.Hobby
{
    public class HobbiesGetAllResponse
    {
        public string Id { get; set; }
        public UserHobbyResponse User { get; set; }
        public GameHobbyResponse Game { get; set; }
    }
}