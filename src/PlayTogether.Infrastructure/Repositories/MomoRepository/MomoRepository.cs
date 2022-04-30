using Newtonsoft.Json.Linq;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Momo;
using System;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories.MomoRepository
{
    public class MomoRepository : IMomoRepository
    {
        public async Task<Result<string>> GenerateMomoLinkAsync()
        {
            var result = new Result<string>();
            string endpoint = "https://test-payment.momo.vn/pay/query-status";
            string partnerCode = "MOMO5RGX20191128";
            string merchantRefId = "1519717410468";
            string version = "2.0";
            string publicKey = "<RSAKeyValue><Modulus>2XH2JFw5YakSagtabOr6Qy/GBy8tY35usAOnHZ08ahIGomMLrS7MPtxK30Foa2AKaF6z/gFqrsBF+IB8yLC7UtYUatPsCz/zzlWR5jP6+SCsjv8l0bXGzPA8O31UVPUnoFFUBfL3K5ORQ8REKjlpRe6EZpLQndVRu93V8LqjOdpp7xT+zhICB9FOEGKHmOR69v+ewubsuLAC88d5ALowopm1zx5DRA6MgBFt0SId108X2JOItJ6y3NlKJhJGC8oXNduUp5SvnlKigH75mqcgBzvA1jvWbRQwDiiIIcBvPh8UXgU8qDOh24rY6Ly0e2leMdO9nZ6aEWKox4fU8otmY2q8RpswuEA0Aq3jz6A/QXy/EoW9rIA4OjfifqhY1eCSIfDAd1/YkgU7n+gxiP21HnDfj/aw9Dj+/rLva+ohy4oWZvfYxHpiCpB8tTBfiHpGCMxik2ejf9qT0Nnx/xP10zW34JSiBX0u0ByV/Ol2X7g/tIfTGRyGIUDqj+DYmO1Tu+WjJli0KBNX0TQvdFNjnvbsLvDxTPKVNSJImpPZb/V/1f8z5fUEEvrC7TNNhuJL+j0OoI15PeFRlUsM7052EiSr08Tgh8yIt2T7Tjbms25ljfM2+glh+UvrqW9RIZm/eNkYfPRQSG3a2kV7y29xebnKX60R4rq3XWpgT2nxGIE=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            string requestId = Guid.NewGuid().ToString();
            string description = "";

            //get hash
            MoMoSecurity momoCrypto = new MoMoSecurity();
            string hash = momoCrypto.buildQueryHash(partnerCode, merchantRefId, requestId,
                publicKey);

            //request to MoMo
            string jsonRequest = "{\"partnerCode\":\"" +
                partnerCode + "\",\"partnerRefId\":\"" +
                merchantRefId + "\",\"description\":\"" +
                description + "\",\"version\":" +
                version + ",\"hash\":\"" +
                hash + "\"}";

            //response from MoMo
            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, jsonRequest.ToString());
            JObject jmessage = JObject.Parse(responseFromMomo);
            result.Content = responseFromMomo;
            return result;
        }
    }
}