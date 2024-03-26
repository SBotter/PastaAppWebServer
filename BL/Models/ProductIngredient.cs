using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    public class ProductIngredient
    {
        public Guid ProductId { get; set; }
        public Guid IngredientId { get; set; }
    }
}
