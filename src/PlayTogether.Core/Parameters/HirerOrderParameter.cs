namespace PlayTogether.Core.Parameters
{
    public class HirerOrderParameter : QueryStringParameters
    {
        public string Status { get; set; }
        public bool? IsNew { get; set; }
    }
}