using System.ComponentModel.DataAnnotations;

namespace CarBaseFinal.Data
{
    public class Sale
    {
        [Key]
        public int OrderID { get; set; }
        public int ClientID { get; set; }


        public string? Mark { get; set; }
        public string? Model { get; set; }
        public string? Type { get; set; }
        public string? Color { get; set; }
        public decimal Price { get; set; }
        public string? ClientName { get; set; }
        public long? ClientPhone { get; set; }

    }
}
