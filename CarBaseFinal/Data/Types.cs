using System.ComponentModel.DataAnnotations;

namespace CarBaseFinal.Data
{
    public partial class Types
    {
        [Key]
        public int TypeID { get; set; }
        public string? Type { get; set; }
    }
}
