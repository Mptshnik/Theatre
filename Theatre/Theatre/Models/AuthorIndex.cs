using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Theatre.Models
{
    public class AuthorIndex
    {
        public IEnumerable<Author> Authors { get; set; }
        public Author Author { get; set; }
        public int CurrentRole { get; set; }
    }
}
