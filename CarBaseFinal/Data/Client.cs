using System.ComponentModel.DataAnnotations;
namespace CarBaseFinal.Data
{
    public partial class Client
    {
        [Key]
        public int ClientID { get; set; }

        public string? ClientName { get; set; }
        public long? ClientPhone{ get; set; }

    }
}
