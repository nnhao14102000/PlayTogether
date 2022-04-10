using PlayTogether.Core.Dtos.Outcoming.Generic;

namespace PlayTogether.Infrastructure.Helpers
{
    public static class ErrorHelpers
    {
        public static Error PopulateError(int code, string type, string message){
            return new Error{
                Code = code,
                Type = type,
                Message = message
            };
        }
    }
}