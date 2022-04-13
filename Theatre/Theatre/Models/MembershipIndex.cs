using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Theatre.Models
{
    public class MembershipIndex
    {
        public IEnumerable<Membership> Memberships { get; set; }
        public Membership Membership { get; set; }
        public int CurrentRole { get; set; }
    }
}
