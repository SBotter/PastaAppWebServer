using BL.Models;
using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVC.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _context;

        public CategoryService(DataContext context)
        {
            _context = context;
        }

        public async Task<Response<Category>> GetCategory(Guid categoryId)
        {
            Response<Category> resp = new Response<Category>();

            if(categoryId == Guid.Empty)
            {
                resp.StatusCode = 404;
                resp.StatusMessage = "Category not found.";
                return resp;
            }

            var query = _context.Categories
                .Where(c => c.CategoryId == categoryId && !c.IsDeleted);

            if(query.Count() > 0 )
            {
                resp.StatusCode = 404;
                resp.StatusMessage = "Category not found.";
                resp.Results = query.ToList();
                return resp;
            }

            resp.StatusCode = 404;
            resp.StatusMessage = "Category not found.";
            return resp;
        }
    }
}
