namespace WebApp.Models
{
    public class UserInterestedOffer
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int OfferId { get; set; }
        public Offer Offer { get; set; }
    }
}
