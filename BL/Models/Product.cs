using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    public class Product
    {
        public Guid CompanyId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? DeletedDate { get; set; }
        //public List<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<ProductPicture> ProductPictures { get; set; } = new List<ProductPicture>();
        public List<ProductPackage> ProductPackages { get; set; } = new List<ProductPackage>();
        public List<Ingredient> ProductIngredients { get; set; } = new List<Ingredient>();
        public List<ProductCookInstruction> ProductCookInstructions { get; set; } = new List<ProductCookInstruction>();

    }
}
