using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BL.Models
{
    public class CookInstruction
    {
        public Guid CompanyId { get; set; }
        public Guid CookInstructionId { get; set; }
        public string CookInstructionDescription { get; set; } = string.Empty;
        public int CookInstructionOrden { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? DeletedDate { get; set; }

        [JsonIgnore]
        public IEnumerable<Product> products { get; set; }
    }
}
