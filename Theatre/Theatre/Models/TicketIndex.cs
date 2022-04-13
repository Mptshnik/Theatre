using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Theatre.Models
{
    public class TicketIndex
    {
        public IEnumerable<Ticket> Tickets { get; set; }
        public Ticket Ticket { get; set; }
        public int CurrentRole { get; set; }
    }
}
