using System;
using PlayTogether.Infrastructure.Entities;

namespace PlayTogether.Infrastructure.Helpers
{
    public static class SearchHistoryHelpers
    {
        public static SearchHistory PopulateSearchHistory(string userId, string searchString){
            return new SearchHistory{
                Id = Guid.NewGuid().ToString(),
                CreatedDate = DateTime.Now,
                UserId = userId,
                SearchString = searchString,
                IsActive = true
            };
        }
    }
}