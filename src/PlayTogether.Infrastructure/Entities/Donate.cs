using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Entities
{
    public class Donate : BaseEntity
    {
        public Order Order { get; set; }
        public string OrderId { get; set; }

        public Player Player { get; set; }
        public string PlayerId { get; set; }

        public Charity Charity { get; set; }
        public string CharityId { get; set; }
    }
}
