using System.ComponentModel.DataAnnotations;

namespace BgService.WebUI.Models
{
    public class Forex
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(3)]
        public string BaseCurrency { get; set; }

        [Required]
        [StringLength(3)]
        public string TargetCurrency { get; set; }

        [Required]
        public decimal Rate { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }
    }
}
