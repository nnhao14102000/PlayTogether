using System;
using PlayTogether.Infrastructure.Entities;

namespace PlayTogether.Infrastructure.Helpers
{
    public static class TransactionHelpers
    {
        public static TransactionHistory PopulateTransactionHistory(string userBalanceId, string operation, float money, string typeOfTransaction, string refId){
            return new TransactionHistory{
                Id = Guid.NewGuid().ToString(),
                CreatedDate = DateTime.Now,
                UserBalanceId = userBalanceId,
                Operation = operation,
                Money = money,
                TypeOfTransaction = typeOfTransaction,
                ReferenceTransactionId = refId
            };
        }
    }
}