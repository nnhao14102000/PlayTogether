namespace PlayTogether.Core.Parameters
{
    public class PlayerOrderParameter : QueryStringParameters
    {
        public string Status { get; set; }
        public bool? IsNew { get; set; }
    }
}