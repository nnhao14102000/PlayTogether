using System;
using PlayTogether.Core.Entities;

namespace PlayTogether.Infrastructure.Helpers
{
    public static class TransactionHelpers
    {
        public static TransactionHistory PopulateTransactionHistory(string userBalanceId, string operation, float money, string typeOfTransaction, string refId){
            return new TransactionHistory{
                Id = Guid.NewGuid().ToString(),
                CreatedDate = DateTime.UtcNow.AddHours(7),
                UserBalanceId = userBalanceId,
                Operation = operation,
                Money = money,
                TypeOfTransaction = typeOfTransaction,
                ReferenceTransactionId = refId
            };
        }
    }
}