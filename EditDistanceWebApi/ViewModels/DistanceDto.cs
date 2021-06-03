using System.ComponentModel.DataAnnotations;

namespace DistanceWebApi.ViewModels
{
    public class DistanceDto
    {
        [Required]
        public string First { get; set; }
        [Required]
        public string Second { get; set; }

    }
}
