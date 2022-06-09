using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public enum Category
    {
        Clothing,
        Electronics,
        [Display(Name = "Home Accessories")]
        HomeAccessories,
        Sport,
    }
}
