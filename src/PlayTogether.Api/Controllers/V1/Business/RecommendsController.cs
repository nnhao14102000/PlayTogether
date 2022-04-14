using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;
using PlayTogether.Core.Dtos.Incoming.Business.Recommend;
using System;
using System.Threading.Tasks;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    public class RecommendsController : BaseController
    {
        private readonly PredictionEnginePool<RecommendData, RecommendPredict> _predictEnginePool;

        public RecommendsController(
            PredictionEnginePool<RecommendData, RecommendPredict> predictEnginePool)
        {
            _predictEnginePool = predictEnginePool;
        }

        /// <summary>
        /// Predict is Player recommend for User
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// 
        /// Sample request:
        ///
        ///     POST /api/play-together/v1/recommends
        ///     {
        ///        "userId": "",
        ///        "playerId": ""
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] RecommendData request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            RecommendPredict prediction = _predictEnginePool.Predict(modelName: "PTORecommenderModel", example: request);
            // string result = "";
            // if (Math.Round(prediction.Score, 1) > 3.5) {
            //     result = "With score: " + Math.Round(prediction.Score, 1) + ", " + "Player " + request.playerId + " is recommended for user " + request.userId;
            //     return Ok(result);
            // }
            // else {
            //     result = "With score: " + Math.Round(prediction.Score, 1) + ", " + "Player " + request.playerId + " is not recommended for user " + request.userId;
            //     return Ok(result);
            // }
            return Ok(Math.Round(prediction.Score, 1));
        }
    }
}