using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;
using PlayTogether.Core.Dtos.Incoming.Business.Recommend;
using System;
using System.Threading.Tasks;
using PlayTogether.Core.Dtos.Outcoming.Business.Recommend;
using System.Linq;

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
        ///     [
        ///         {
        ///             "userId": "",
        ///             "playerId": ""
        ///         },
        ///         {
        ///             "userId": "",
        ///             "playerId": ""
        ///         },
        ///     ]
        ///
        /// </remarks>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] List<RecommendData> request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var result = new List<RecommendResult>();
            foreach (var item in request) {
                RecommendPredict prediction = _predictEnginePool.Predict(modelName: "PTORecommenderModel", example: item);
                var recommendResult = new RecommendResult{
                    PlayerId = item.playerId, 
                    Score = Math.Round(prediction.Score, 1).ToString()
                };
                result.Add(recommendResult);
            }
            result.AsQueryable().OrderByDescending(x => x.Score);
            return Ok(result);
        }
    }
}