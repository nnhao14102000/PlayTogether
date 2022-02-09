using PlayTogether.Core.Dtos.Outcoming.Business.Music;

namespace PlayTogether.Core.Dtos.Outcoming.Business.MusicOfPlayer
{
    public class MusicOfPlayerGetAllResponse
    {
        public string Id { get; set; }
        
        public string MusicId { get; set; }
        public MusicGetByIdResponse Music { get; set; }
    }
}