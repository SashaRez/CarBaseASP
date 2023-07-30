using System.ComponentModel.DataAnnotations;

namespace CarBaseFinal.Data
{
    public partial class Marks
    {
        [Key]
        public int MarkId { get; set; }
        public string? Marka { get; set; }
    }
}
