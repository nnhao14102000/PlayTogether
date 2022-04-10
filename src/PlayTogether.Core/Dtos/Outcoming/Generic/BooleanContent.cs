namespace PlayTogether.Core.Dtos.Outcoming.Generic
{
    public class BooleanContent
    {
        public BooleanContent(string message)
        {
            Message = message;
        }
        public string Message { get; set; }
    }
}