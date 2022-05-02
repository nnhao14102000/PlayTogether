namespace PlayTogether.Core.Dtos.Incoming.Momo
{
    public class MomoIPNRequest
    {
        public string PartnerCode { get; set; }
        public string OrderId { get; set; }
        public string RequestId { get; set; }
        public float Amount { get; set; }
        public string OrderInfo { get; set; }
        public string OrderType { get; set; }
        public double TransId { get; set; }
        public int ResultCode { get; set; }
        public string Message { get; set; }
        public string PayType { get; set; }
        public double ResponseTime { get; set; }
        public string ExtraData { get; set; }
        public string Signature { get; set; }
    }
}