using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AgregatorLinkow.Models
{
    public class Link
    {
        public int Id { get; set; }
        [Required] public string Title { get; set; }
        [Url] [Required] public string URL { get; set; }
        public int Points { get; set; }
        public DateTime UploadDate { get; set; }
        public User User { get; set; }
        public IEnumerable<Vote> Votes { get; set; }
    }
}