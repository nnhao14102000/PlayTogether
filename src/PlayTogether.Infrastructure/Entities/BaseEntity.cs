using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Infrastructure.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public String Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdateDate { get; set; }
    }
}