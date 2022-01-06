using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlayTogether.Infrastructure.Data;
using System;

namespace PlayTogether.Api.Helpers
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var dbContext = new AppDbContext(
                    serviceProvider.GetRequiredService<
                        DbContextOptions<AppDbContext>>())) {
                dbContext.Database.EnsureCreated();
            }
        }
    }
}