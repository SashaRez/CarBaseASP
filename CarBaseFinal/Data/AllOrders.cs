using System.ComponentModel.DataAnnotations;
namespace CarBaseFinal.Data
{
    public partial class AllOrders
    {
        [Key]
        public int OrderID { get; set; }

        public int CarID { get; set; }
        public int ClientID { get; set; }
        public string? ClientName { get; set; }
        public long ClientPhone { get; set; }
    }
}
