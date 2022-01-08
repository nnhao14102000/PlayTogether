using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Infrastructure.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        [Column(TypeName = "nvarchar(100)")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column(TypeName = "datetime")]
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "datetime")]
        public DateTime UpdateDate { get; set; }
    }
}