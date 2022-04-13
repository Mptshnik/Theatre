using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Theatre.Models
{
    public class PostIndex
    {
        public IEnumerable<Post> Posts { get; set; }
        public Post Post { get; set; }
        public int CurrentRole { get; set; }
    }
}
