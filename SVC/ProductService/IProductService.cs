using BL.DTO;
using BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVC.ProductService
{
    public interface IProductService
    {
        Task<Response<Product>> AddProduct(ProductDto product);
        Task<Response<Product>> GetProducts(Guid CompanyId);
    }
}
