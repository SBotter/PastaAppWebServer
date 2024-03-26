using BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTO
{
    public class ProductDto
    {
        public Guid CompanyId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? DeletedDate { get; set; }
        public List<CategoryDto> ProductCategories { get; set; } = new List<CategoryDto>();
        public List<ProductPackageDto> ProductPackages { get; set; } = new List<ProductPackageDto>();
        public List<ProductPictureDto> ProductPictures { get; set; } = new List<ProductPictureDto>();
        public List<ProductIngredientDto> ProductIngredients { get; set; } = new List<ProductIngredientDto>();
        public List<ProductCookInstructionDto> ProductCookInstructions { get; set;} = new List<ProductCookInstructionDto>();
    }
}
