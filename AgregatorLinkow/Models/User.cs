using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace AgregatorLinkow.Models
{
    public class User : IdentityUser
    {
        public ICollection<Link> Links { get; set; }
        public ICollection<Vote> Votes { get; set; }
    }
}
