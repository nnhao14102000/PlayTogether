namespace PlayTogether.Core.Parameters
{
    public class CharityParameters : QueryStringParameters
    {
        public string Name { get; set; }
        public bool? IsActive { get; set; }
    }
}