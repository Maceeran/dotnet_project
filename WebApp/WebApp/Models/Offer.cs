using System.ComponentModel.DataAnnotations;

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
    }
}
