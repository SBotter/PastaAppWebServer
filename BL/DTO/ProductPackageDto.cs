using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTO
{
    public class ProductPackageDto
    {
        public Guid PackageId { get; set; }
        public string PackageName { get; set; } = string.Empty;
        public string PackageDescription { get; set; } = string.Empty;
        public string PackageUnit { get; set; } = string.Empty;
        public float PackageSize { get; set; }
        public float PackagePrice { get; set; }
    }
}
