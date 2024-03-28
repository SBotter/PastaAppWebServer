using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BL.Models
{
    public class ProductCategory
    {
        public Guid ProductId { get; set; }

        public Guid CategoryId { get; set; }

        [JsonIgnore]
        public Category Category { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }
    }
}
