using System.ComponentModel.DataAnnotations;

namespace MVC_FlowerShop.Models
{
    public class Flower
    {
        [Key] ///primary key
        public int FlowerID {  get; set; }
        public int FlowerName { get; set; }
        public int FlowertType { get; set; }

        public int FlowerPrice { get; set; }

        public int FlowerPurchaseDate { get; set; }

    }
}
