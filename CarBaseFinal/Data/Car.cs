using System.ComponentModel.DataAnnotations;

namespace CarBaseFinal.Data
{

    // "?" - переменной разрешается хранить "null".

    public class Car
    {
        [Key]
        public int CarID { get; set; }

        public int TypeID { get; set; }
        public string? Type { get; set; }
        public int MarkID { get; set; }
        public string? Mark { get; set; }
        public string? Model { get; set; }
        public string? Color { get; set; }
        public decimal Price { get; set; }
    }
}
