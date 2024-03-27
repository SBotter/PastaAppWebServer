using BL.DTO;
using BL.Models;
using DAL.Data;
using Microsoft.EntityFrameworkCore.Storage;
using SVC.CategoryService;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SVC.ProductService
{
    public class ProductService: IProductService
    {
        private readonly DataContext _context;
        private readonly ICategoryService _categoryService;

        public ProductService(DataContext context, ICategoryService categoryService)
        {
            _context = context;
            _categoryService = categoryService;
        }

        #region POST

        //ADD NEW PRODUCT
        public async Task<Response<Product>> AddProduct(ProductDto product)
        {
            Response<Product> resp = new Response<Product>();

            if(product == null) {
                resp.StatusCode = 404;
                resp.StatusMessage = "Product not found.";
                return resp;
            }

            if(product.ProductCategories.Count == 0) {
                resp.StatusCode = 404;
                resp.StatusMessage = "Category not found.";
                return resp;
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {

                    Product prod = new Product();
                    prod.CompanyId = product.CompanyId;
                    prod.ProductId = Guid.NewGuid();
                    prod.ProductName = product.ProductName;
                    prod.ProductDescription = product.ProductDescription;
                    prod.IsDeleted = false;
                    prod.CreatedDate = DateTime.Now;
                    prod.DeletedDate = null;

                    _context.Products.Add(prod);
                    await _context.SaveChangesAsync();

                    //save the category list
                    if(product.ProductCategories.Count > 0)
                    {
                        Response<Category> responseCategory = new Response<Category>();
                        responseCategory = await AddProductCategory(product.ProductCategories, prod.ProductId);
                        if (responseCategory.StatusCode != 200)
                        {
                            resp.StatusCode = 500;
                            resp.StatusMessage = "Error: Category not saved.";
                            return resp;
                        }
                        prod.Categories.AddRange(responseCategory.Results);
                    }

                    //add pictures
                    if (product.ProductPictures.Count > 0)
                    {
                        Response<ProductPicture> responsePicture = new Response<ProductPicture>();
                        responsePicture = await AddProductPicture(product.ProductPictures, prod.ProductId);
                        if(responsePicture.StatusCode != 200)
                        {
                            resp.StatusCode = 500;
                            resp.StatusMessage = "Error: Pictures not saved.";
                            return resp;
                        }
                    }

                    //add packages
                    if(product.ProductPackages.Count > 0)
                    {
                        Response<ProductPackage> responsePackage = new Response<ProductPackage>();
                        responsePackage = await AddProductPackage(product.ProductPackages, prod.ProductId);
                        if (responsePackage.StatusCode != 200)
                        {
                            resp.StatusCode = 500;
                            resp.StatusMessage = "Error: Packages not saved.";
                            return resp;
                        }
                    }

                    //add ingredients
                    if (product.ProductIngredients.Count > 0)
                    {
                        Response<ProductIngredient> responseIngredient = new Response<ProductIngredient>();
                        responseIngredient = await AddProductIngredient(product.ProductIngredients, prod.ProductId);
                        if (responseIngredient.StatusCode != 200)
                        {
                            resp.StatusCode = 500;
                            resp.StatusMessage = "Error: Ingredient not saved.";
                            return resp;
                        }
                    }

                    //add Cook Instructions
                    if(product.ProductCookInstructions.Count > 0)
                    {
                        Response<ProductCookInstruction> responseInstruction = new Response<ProductCookInstruction>();
                        responseInstruction = await AddProductCookInstruction(product.ProductCookInstructions, prod.ProductId);
                        if (responseInstruction.StatusCode != 200)
                        {
                            resp.StatusCode = 500;
                            resp.StatusMessage = "Error: Cook Instruction not saved.";
                            return resp;
                        }
                    }

                    await transaction.CommitAsync();

                    resp.StatusCode = 200;
                    resp.StatusMessage = "Success.";
                    resp.Results.Add(prod);

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    resp.StatusCode = 500;
                    resp.StatusMessage = $"Error: {ex.Message}";
                }
            }

            return resp;
        }

        private async Task<Response<Category>> AddProductCategory(List<CategoryDto> categories, Guid productId)
        {
            Response<Category> resp = new Response<Category>();

            if (categories == null || categories.Count == 0)
            {
                resp.StatusCode = 404;
                resp.StatusMessage = "Category not found.";
                return resp;
            }

            if (productId == Guid.Empty)
            {
                resp.StatusCode = 404;
                resp.StatusMessage = "Product not found.";
                return resp;
            }

            //save the Productcategories table
            foreach (var cat in categories)
            {
                ProductCategory prodCategory = new ProductCategory();
                prodCategory.ProductId = productId;
                prodCategory.CategoryId = cat.CategoryId;

                _context.ProductCategories.Add(prodCategory);
            }

            _context.SaveChanges();

            Response<Category> category = new Response<Category>();
            foreach (var cat in categories)
            {
                category = await _categoryService.GetCategory(cat.CategoryId);
                if (category != null)
                {
                    resp.Results.AddRange(category.Results);
                }
            }

            resp.StatusCode = 200;
            resp.StatusMessage = "Success.";

            return resp;
        }

        private async Task<Response<ProductPicture>> AddProductPicture(List<ProductPictureDto> productPictures, Guid productId)
        {
            Response<ProductPicture> resp = new Response<ProductPicture>();

            if(productPictures == null || productPictures.Count == 0)
            {
                resp.StatusCode = 404;
                resp.StatusMessage = "Picture not found.";
                return resp;
            }

            if(productId == Guid.Empty)
            {
                resp.StatusCode = 404;
                resp.StatusMessage = "Product not found.";
                return resp;
            }

            foreach (var pict in productPictures)
            {
                ProductPicture pPicture = new ProductPicture();
                pPicture.PictureId = Guid.NewGuid();
                pPicture.ProductId = productId;
                pPicture.PictureUrl = pict.PictureUrl;
                pPicture.IsDeleted = false;
                pPicture.CreatedDate = DateTime.Now;
                pPicture.DeletedDate = null;

                _context.ProductPictures.Add(pPicture);
                resp.Results.Add(pPicture);
            }

            _context.SaveChanges();

            resp.StatusCode = 200;
            resp.StatusMessage = "Success.";
            
            return resp;       
        }

        private async Task<Response<ProductPackage>> AddProductPackage(List<ProductPackageDto> productPackages, Guid productId)
        {
            Response<ProductPackage> resp = new Response<ProductPackage>();

            if (productPackages == null || productPackages.Count == 0)
            {
                resp.StatusCode = 404;
                resp.StatusMessage = "Package not found.";
                return resp;
            }

            if (productId == Guid.Empty)
            {
                resp.StatusCode = 404;
                resp.StatusMessage = "Product not found.";
                return resp;
            }

            foreach(var pack in productPackages)
            {
                ProductPackage productPackage = new ProductPackage();
                productPackage.ProductId = productId;
                productPackage.PackageId = Guid.NewGuid();
                productPackage.PackageName = pack.PackageName;
                productPackage.PackageDescription = pack.PackageDescription;
                productPackage.PackageUnit = pack.PackageUnit;
                productPackage.PackageSize = pack.PackageSize;
                productPackage.PackagePrice = pack.PackagePrice;
                productPackage.IsDeleted = false;
                productPackage.CreatedDate = DateTime.Now;
                productPackage.DeletedDate = null;

                _context.ProductPackages.Add(productPackage);
                resp.Results.Add(productPackage);
            }

            _context.SaveChanges();

            resp.StatusCode = 200;
            resp.StatusMessage = "Success.";

            return resp;
        }

        private async Task<Response<ProductIngredient>> AddProductIngredient(List<ProductIngredientDto> ingredients, Guid productId)
        {
            Response<ProductIngredient> resp = new Response<ProductIngredient>();

            if(ingredients == null || ingredients.Count == 0)
            {
                resp.StatusCode = 404;
                resp.StatusMessage = "Ingredient not found.";
                return resp;
            }

            if (productId == Guid.Empty)
            {
                resp.StatusCode = 404;
                resp.StatusMessage = "Product not found.";
                return resp;
            }

            foreach(var ingredient in ingredients)
            {
                ProductIngredient ing = new ProductIngredient();
                ing.ProductId = productId;
                ing.IngredientId = ingredient.IngredientId;

                _context.ProductIngredients.Add(ing);
            }

            _context.SaveChanges();

            resp.StatusCode = 200;
            resp.StatusMessage = "Success.";

            return resp;
        }

        private async Task<Response<ProductCookInstruction>> AddProductCookInstruction(List<ProductCookInstructionDto> cookInstructions, Guid productId)
        {
            Response<ProductCookInstruction> resp = new Response<ProductCookInstruction>();

            if (cookInstructions == null || cookInstructions.Count == 0)
            {
                resp.StatusCode = 404;
                resp.StatusMessage = "Cook Instructions not found.";
                return resp;
            }

            if (productId == Guid.Empty)
            {
                resp.StatusCode = 404;
                resp.StatusMessage = "Product not found.";
                return resp;
            }

            foreach(var cookInstruction in cookInstructions)
            {
                ProductCookInstruction instruction = new ProductCookInstruction();
                instruction.ProductId = productId;
                instruction.CookInstructionId = cookInstruction.CookInstructionId;

                _context.ProductCookInstructions.Add(instruction);
            }

            _context.SaveChanges();

            resp.StatusCode = 200;
            resp.StatusMessage = "Success.";

            return resp;
        }

        #endregion

        #region GET

        //LIST THE PRODUCTS
        public async Task<Response<Product>> GetProducts(Guid companyId)
        {
            List<Product> products = new List<Product>();
            
            Response<Product> resp = new Response<Product>();

            if (companyId == Guid.Empty)
            {
                resp.StatusCode = 404;
                resp.StatusMessage = "Comapany not found.";
                return resp;
            }

            var query = _context.Products
                .Where(p => !p.IsDeleted && p.CompanyId == companyId)
                .OrderBy(p => p.ProductName);

            if(query.Count() == 0)
            {
                resp.StatusCode = 404;
                resp.StatusMessage = "Product not found.";
                return resp;
            }

            products = query.ToList();

            foreach(var prod in products)
            {
                List<Category> categories = await GetProductCategory(prod.ProductId, companyId);
                prod.Categories = categories;
            }

            


            resp.StatusCode = 200;
            resp.StatusMessage = "Success.";
            resp.Results = query.ToList();

            return resp;
        }

        public async Task<List<Category>> GetProductCategory(Guid productId, Guid companyId)
        {
            Response<Product> resp = new Response<Product>();

            if (productId == Guid.Empty || companyId == Guid.Empty) return null;
            
            var query = _context.Categories
                .Join(_context.ProductCategories, c => c.CategoryId, pc => pc.CategoryId, (c, pc) => new { Category = c, ProductCategory = pc })
                .Where(j => j.Category.CompanyId == companyId && !j.Category.IsDeleted && j.ProductCategory.ProductId == productId)
                .Select(j => j.Category);

            
            if(query.Count() > 0)
            {
                return query.ToList();
            }

            return null;
            

        }

        #endregion
    }
}
