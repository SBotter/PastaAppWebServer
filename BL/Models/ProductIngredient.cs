using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BL.Models
{
    public class ProductIngredient
    {
        public Guid ProductId { get; set; }
        public Guid IngredientId { get; set; }

        [JsonIgnore]
        public Ingredient Ingredient { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }
    }
}
