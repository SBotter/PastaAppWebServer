using BL.Models;
using BL.DTO.Category;
using DAL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
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

        public async Task<Response<Category>> AddCategory(string categoryName, Guid companyId)
        {
            Response<Category> resp = new Response<Category>();

            if (string.IsNullOrEmpty(categoryName))
            {
                resp.StatusCode = 404;
                resp.StatusMessage = "Category not found.";
                return resp;
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Category category = new Category();
                    category.CompanyId = companyId;
                    category.CategoryId = Guid.NewGuid();
                    category.CategoryName = categoryName;
                    category.IsDeleted = false;
                    category.CreatedDate = DateTime.Now;
                    category.DeletedDate = null;

                    _context.Categories.Add(category);
                    _context.SaveChanges();

                    await transaction.CommitAsync();

                    resp.StatusCode = 200;
                    resp.StatusMessage = "Success.";
                    resp.Results.Add(category);

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
                resp.StatusCode = 200;
                resp.StatusMessage = "Success.";
                resp.Results = query.ToList();
                return resp;
            }

            resp.StatusCode = 404;
            resp.StatusMessage = "Category not found.";
            return resp;
        }

        public async Task<Response<Category>> GetCategories(Guid companyId)
        {
            Response<Category> resp = new Response<Category>();
            
            if(companyId == Guid.Empty)
            {
                resp.StatusCode = 404;
                resp.StatusMessage = "Company not found.";
                return resp;
            }

            var query = _context.Categories
                .Where(c => c.CompanyId == companyId && !c.IsDeleted)
                .OrderBy(c => c.CategoryName);

            if(query.Count() > 0 )
            {
                resp.StatusCode = 200;
                resp.StatusMessage = "Success.";
                resp.Results = query.ToList();
                return resp;
            }

            resp.StatusCode = 404;
            resp.StatusMessage = "Category not found.";
            return resp;
        }

        public async Task<Response<Category>> UpdateCategory(CategoryDto category)
        {
            Response<Category> resp = new Response<Category>();

            if (category == null || category.CategoryId == Guid.Empty || string.IsNullOrEmpty(category.CategoryName))
            {
                resp.StatusCode = 404;
                resp.StatusMessage = "Category not found.";
                return resp;
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var categoryToUpdate = _context.Categories
                        .SingleOrDefault(c => c.CategoryId == category.CategoryId);

                    if (categoryToUpdate != null)
                    {
                        categoryToUpdate.CategoryName = category.CategoryName;

                        _context.SaveChanges();


                        await transaction.CommitAsync();

                        resp.StatusCode = 200;
                        resp.StatusMessage = "Success.";
                        resp.Results.Add(categoryToUpdate);

                    }
                    else
                    {
                        resp.StatusCode = 404;
                        resp.StatusMessage = "Category not saved.";
                    }

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

        public async Task<Response<Category>> DeleteCategory(Guid categoryId)
        {
            Response<Category> resp = new Response<Category>();

            if (categoryId == Guid.Empty)
            {
                resp.StatusCode = 404;
                resp.StatusMessage = "Category not found.";
                return resp;
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var categoryToUpdate = _context.Categories
                        .SingleOrDefault(c => c.CategoryId == categoryId);

                    if (categoryToUpdate != null)
                    {
                        categoryToUpdate.IsDeleted = true;
                        categoryToUpdate.DeletedDate = DateTime.Now;

                        _context.SaveChanges();

                        await transaction.CommitAsync();

                        resp.StatusCode = 200;
                        resp.StatusMessage = "Success.";
                        resp.Results.Add(categoryToUpdate);

                    }
                    else
                    {
                        resp.StatusCode = 404;
                        resp.StatusMessage = "Category not deleted.";
                    }

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
    }
}
