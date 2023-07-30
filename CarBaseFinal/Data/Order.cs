using System.ComponentModel.DataAnnotations;
namespace CarBaseFinal.Data
{
    public partial class Order
    {
        [Key]
        public int CarID { get; set; }

        public string? Mark { get; set; }
        public string? Model { get; set; }
        public string? Type { get; set; }
        public string? Color { get; set; }
        public int Kol { get; set; }
        public decimal Price { get; set; }

    }
}
