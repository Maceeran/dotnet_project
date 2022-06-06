using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class Photo
    {
        public int Id { get; set; }
        [ForeignKey("Offer")]
        public int OfferId { get; set; }
        [Display(Name = "Photo")]
        [DataType(DataType.Upload)]
        public IFormFile PhotoFile { get; set; } 
    }
}
