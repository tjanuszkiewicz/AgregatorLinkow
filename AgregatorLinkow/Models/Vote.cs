namespace AgregatorLinkow.Models
{
    public class Vote
    {
        public int Id { get; set; }
        public Link Link { get; set; }
        public User User { get; set; }
        public bool Voted { get; set; }
    }
}