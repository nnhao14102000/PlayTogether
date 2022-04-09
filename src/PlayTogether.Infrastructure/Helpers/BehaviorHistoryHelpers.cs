using System;
using PlayTogether.Infrastructure.Entities;

namespace PlayTogether.Infrastructure.Helpers
{
    public static class BehaviorHistoryHelpers
    {
        public static BehaviorHistory PopulateBehaviorHistory(string behaviorPointId, string operation, string type, string referenceId){
            return new BehaviorHistory{
                Id = Guid.NewGuid().ToString(),
                CreatedDate = DateTime.UtcNow.AddHours(7),
                BehaviorPointId = behaviorPointId,
                Operation = operation,
                Type = type,
                ReferenceBehaviorId = referenceId
            };
        }
    }
}