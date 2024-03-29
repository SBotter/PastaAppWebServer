using BL.DTO;
using BL.DTO.Category;
using BL.Models;
using DAL.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SVC.CategoryService;

namespace WebAPICodeFirst.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        [Route("AddCategory")]
        public async Task<Response<Category>> AddCategory(string categoryName)
        {
            Guid companyId = new Guid("8454ADAA-6414-4007-A337-25BEFC00CF66");
            Response<Category> response = await _categoryService.AddCategory(categoryName, companyId);
            return response;
        }

        [HttpGet]
        [Route("GetCategories")]
        public async Task<Response<Category>> GetCategories()
        {
            Guid companyId = new Guid("8454ADAA-6414-4007-A337-25BEFC00CF66");
            Response<Category> response = await _categoryService.GetCategories(companyId);
            return response;
        }

        [HttpGet]
        [Route("GetCategory/{Id}")]
        public async Task<Response<Category>> GetCategory(Guid Id)
        {
            Response<Category> response = await _categoryService.GetCategory(Id);
            return response;
        }

        [HttpPut]
        [Route("UpdateCategory")]
        public async Task<Response<Category>> UpdateCategory(CategoryDto category )
        {
            Response<Category> response = await _categoryService.UpdateCategory(category);
            return response;
        }

        [HttpDelete]
        [Route("DeleteCategory/{Id}")]
        public async Task<Response<Category>> DeleteCategory(Guid Id)
        {
            Response<Category> response = await _categoryService.DeleteCategory(Id);
            return response;
        }


    }
}
