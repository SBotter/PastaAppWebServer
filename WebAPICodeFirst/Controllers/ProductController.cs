using BL.DTO.Product;
using BL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SVC.ProductService;

namespace WebAPICodeFirst.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        [Route("AddProduct")]
        public async Task<Response<Product>> AddProduct(ProductDto product)
        {
            Response<Product> response = await _productService.AddProduct(product);
            return response;
        }

        [HttpGet]
        [Route("GetProducts")]
        public async Task<Response<Product>> GetProducts()
        {
            Guid companyId = new Guid("8454ADAA-6414-4007-A337-25BEFC00CF66");
            Response<Product> response = await _productService.GetProducts(companyId);
            return response;
        }

        [HttpGet]
        [Route("GetProducts/{Id}")]
        public async Task<Response<Product>> GetProduct(Guid Id)
        {
            Guid companyId = new Guid("8454ADAA-6414-4007-A337-25BEFC00CF66");
            Response<Product> response = await _productService.GetProduct(Id);
            return response;
        }
    }
}
