using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BL.Models
{
    public class ProductCookInstruction
    {
        public Guid ProductId { get; set; }
        public Guid CookInstructionId { get; set; }
        
        [JsonIgnore]
        public CookInstruction CookInstruction { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }
    }
}
