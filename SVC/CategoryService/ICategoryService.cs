using BL.DTO.Category;
using BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVC.CategoryService
{
    public interface ICategoryService
    {
        Task<Response<Category>> AddCategory(string categoryName, Guid companyId);
        Task<Response<Category>> GetCategory(Guid categoryId);
        Task<Response<Category>> GetCategories(Guid companyId);
        Task<Response<Category>> UpdateCategory(CategoryDto category);
        Task<Response<Category>> DeleteCategory(Guid categoryId);
    }
}
