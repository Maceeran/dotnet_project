using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApp.Models
{
    public class Photo
    {
        public int Id { get; set; }

        [ForeignKey("Offer")]
        public int OfferId { get; set; }
        public Offer? Offer { get; set; }

        public string? FileAddress { get; set; }

        [NotMapped]
        public IFormFile? File { get; set; } 
    }
}
