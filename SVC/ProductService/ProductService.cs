using BL.DTO;
using BL.Models;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SVC.CategoryService;
using SVC.CookInstructionService;
using SVC.IngredientService;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
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
        private readonly IIngredientService _ingredientService;
        private readonly ICookInstructionService _cookInstructionService;

        public ProductService(DataContext context, ICategoryService categoryService, IIngredientService ingredientService, ICookInstructionService cookInstructionService)
        {
            _context = context;
            _categoryService = categoryService;
            _ingredientService = ingredientService;
            _cookInstructionService = cookInstructionService;
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
                        Response<Ingredient> responseIngredient = new Response<Ingredient>();
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
                        Response<CookInstruction> responseInstruction = new Response<CookInstruction>();
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

        private async Task<Response<Ingredient>> AddProductIngredient(List<ProductIngredientDto> ingredients, Guid productId)
        {
            Response<Ingredient> resp = new Response<Ingredient>();

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

            foreach(var ing in ingredients)
            {
                ProductIngredient prodIngredient = new ProductIngredient();
                prodIngredient.ProductId = productId;
                prodIngredient.IngredientId = ing.IngredientId;

                _context.ProductIngredients.Add(prodIngredient);
            }

            _context.SaveChanges();

            Response<Ingredient> ingredient = new Response<Ingredient>();
            foreach (var ing in ingredients)
            {
                ingredient = await _ingredientService.GetIngredient(ing.IngredientId);
                if (ingredient != null)
                {
                    resp.Results.AddRange(ingredient.Results);
                }
            }

            resp.StatusCode = 200;
            resp.StatusMessage = "Success.";

            return resp;
        }

        private async Task<Response<CookInstruction>> AddProductCookInstruction(List<ProductCookInstructionDto> cookInstructions, Guid productId)
        {
            Response<CookInstruction> resp = new Response<CookInstruction>();

            if (cookInstructions == null || cookInstructions.Count == 0)
            {
                resp.StatusCode = 404;
                resp.StatusMessage = "Cook Instruction not found.";
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
         
            Response<CookInstruction> cookInst = new Response<CookInstruction>();
            foreach (var ci in cookInstructions)
            {
                cookInst = await _cookInstructionService.GetCookInstruction(ci.CookInstructionId);
                if (cookInst != null)
                {
                    resp.Results.AddRange(cookInst.Results);
                }
            }

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
                .Join(
                    _context.Companies.Where(c => c.CompanyId == companyId && !c.IsDeleted ), 
                    p => p.CompanyId, 
                    c => c.CompanyId, 
                    (p, c) => p
                )
                .Where(p => !p.IsDeleted)
                .OrderBy(p => p.ProductName);

            if (query.Count() == 0)
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

                List<ProductPicture> pictures = await GetProductPicture(prod.ProductId);
                prod.ProductPictures = pictures;

                List<ProductPackage> packages = await GetProductPackage(prod.ProductId);
                prod.ProductPackages = packages;

                List<Ingredient> ingredients = await GetProductIngredient(prod.ProductId, companyId);
                prod.ProductIngredients = ingredients;

                List<CookInstruction> cookInstructions = await GetProductCookInstruction(prod.ProductId, companyId);
                prod.ProductCookInstructions = cookInstructions;

            }


            resp.StatusCode = 200;
            resp.StatusMessage = "Success.";
            resp.Results = query.ToList();

            return resp;
        }

        private async Task<List<Category>> GetProductCategory(Guid productId, Guid companyId)
        {
            List<Category> categories = new List<Category>();

            if (productId == Guid.Empty || companyId == Guid.Empty)
            {
                return categories;
            }
            
            var query = _context.Categories
                .Join(_context.ProductCategories, c => c.CategoryId, pc => pc.CategoryId, (c, pc) => new { Category = c, ProductCategory = pc })
                .Where(j => j.Category.CompanyId == companyId && !j.Category.IsDeleted && j.ProductCategory.ProductId == productId)
                .Select(j => j.Category);

            if(query.Count() > 0)
            {
                categories = query.ToList();
            }

            return categories;           
        }

        private async Task<List<ProductPicture>> GetProductPicture(Guid productId)
        {
            List<ProductPicture> pictures = new List<ProductPicture>();

            if(productId == Guid.Empty)
            {
                return pictures;
            }

            var query = _context.ProductPictures
                .Where(pp => pp.ProductId == productId && !pp.IsDeleted);

            if(query.Count() > 0)
            {
                pictures = query.ToList();
            }

            return pictures;
        }

        private async Task<List<ProductPackage>> GetProductPackage(Guid productId)
        {
            List<ProductPackage> packages = new List<ProductPackage>();

            if (productId == Guid.Empty)
            {
                return packages;
            }

            var query = _context.ProductPackages
                .Where(pp => pp.ProductId == productId && !pp.IsDeleted);

            if (query.Count() > 0)
            {
                packages = query.ToList();
            }

            return packages;
        }

        private async Task<List<Ingredient>> GetProductIngredient(Guid productId, Guid companyId)
        {
            List<Ingredient> ingredients = new List<Ingredient>();

            if (productId == Guid.Empty || companyId == Guid.Empty)
            {
                return ingredients;
            }

            var query = _context.Ingredients
                .Join(
                    _context.ProductIngredients
                        .Where(prodIng => prodIng.ProductId == productId),
                    i => i.IngredientId,
                    prodIng => prodIng.IngredientId,
                    (i, prodIng) => i
                )
                .Where(i => i.CompanyId == companyId && !i.IsDeleted);

            if (query.Count() > 0)
            {
                ingredients = query.ToList();
            }

            return ingredients;

        }

        private async Task<List<CookInstruction>> GetProductCookInstruction(Guid productId, Guid companyId)
        {
            List<CookInstruction> cookInstructions = new List<CookInstruction>();

            if (productId == Guid.Empty || companyId == Guid.Empty)
            {
                return cookInstructions;
            }

            var query = _context.CookInstructions
                .Join(
                    _context.ProductCookInstructions
                        .Where(pci => pci.ProductId == productId),
                    ci => ci.CookInstructionId,
                    pci => pci.CookInstructionId,
                    (ci, pci) => ci
                )
                .Where(ci => ci.CompanyId == companyId && !ci.IsDeleted);
            
            if (query.Count() > 0)
            {
                cookInstructions = query.ToList();
            }

            return cookInstructions;

        }
    
        #endregion
    }
}
