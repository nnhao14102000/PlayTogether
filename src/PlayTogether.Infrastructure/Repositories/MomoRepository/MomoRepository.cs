using Newtonsoft.Json.Linq;
using PlayTogether.Core.Dtos.Incoming.Momo;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Momo;
using System;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories.MomoRepository
{
    public class MomoRepository : IMomoRepository
    {
        public async Task<Result<JObject>> GenerateMomoLinkAsync(WebPaymentRequest request)
        {
            var result = new Result<JObject>();
            //request params need to request to MoMo system
            string endpoint = "https://test-payment.momo.vn/v2/gateway/api/create";
            string partnerCode = "MOMOVFTR20220325";
            string accessKey = "mZEfBbA18qzzbgNg";
            string serectkey = "l7YkkLd9pQZR7Kk08bsEMeK2Grk7x20e";
            string orderInfo = "NẠP TIỀN PLAY TOGETHER";
            string redirectUrl = "https://playtogether.page.link";
            //https://webhook.site/b3088a6a-2d17-4f8d-a383-71389a6c600b
            string ipnUrl = "https://play-together.azurewebsites.net/api/play-together/v1/momo/ipn";
            string requestType = "captureWallet";

            string amount = request.Amount.ToString();
            string orderId = request.UserId + "_" +  Guid.NewGuid().ToString();
            string requestId = Guid.NewGuid().ToString();
            string extraData = "";

            //Before sign HMAC SHA256 signature
            string rawHash = "accessKey=" + accessKey +
                "&amount=" + amount +
                "&extraData=" + extraData +
                "&ipnUrl=" + ipnUrl +
                "&orderId=" + orderId +
                "&orderInfo=" + orderInfo +
                "&partnerCode=" + partnerCode +
                "&redirectUrl=" + redirectUrl +
                "&requestId=" + requestId +
                "&requestType=" + requestType
                ;


            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);

            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "partnerName", "PlayTogether" },
                { "storeId", "MomoTestStore" },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderId },
                { "orderInfo", orderInfo },
                { "redirectUrl", redirectUrl },
                { "ipnUrl", ipnUrl },
                { "lang", "en" },
                { "extraData", extraData },
                { "requestType", requestType },
                { "signature", signature }

            };
            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

            JObject jmessage = JObject.Parse(responseFromMomo);
            result.Content = jmessage;
            return result;
        }
    }
}