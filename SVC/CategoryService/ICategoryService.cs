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
        Task<Response<Category>> GetCategory(Guid categoryId);
    }
}
