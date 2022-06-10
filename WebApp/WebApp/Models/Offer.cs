using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Models
{
    public class Offer
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string RetrievalAddress { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime VoidDate { get; set; }
        public List<Photo>? Photos { get; set; }
        public Category Category { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public List<UserInterestedOffer>? InterestedUsers { get; set; }
        public bool isRealized { get; set; }

        public Offer()
        {
            isRealized = false;
        }
    }
}
