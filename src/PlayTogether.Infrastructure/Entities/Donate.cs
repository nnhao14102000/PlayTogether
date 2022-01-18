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

        public Player PlayerMakeDonate { get; set; }
        public string PlayerMakeDonateId { get; set; }

        public Charity CharityReceiveDonate { get; set; }
        public string CharityReceiveDonateId { get; set; }
    }
}
