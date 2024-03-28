using BL.Models;
using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVC.IngredientService
{
    public class IngredientService:IIngredientService
    {
        private readonly DataContext _context;

        public IngredientService(DataContext context)
        {
            _context = context;
        }

        public async Task<Response<Ingredient>> GetIngredient(Guid ingredientId)
        {
            Response<Ingredient> resp = new Response<Ingredient>();

            if (ingredientId == Guid.Empty)
            {
                resp.StatusCode = 404;
                resp.StatusMessage = "Ingredient not found.";
                return resp;
            }

            var query = _context.Ingredients
                .Where(i => i.IngredientId == ingredientId && !i.IsDeleted);

            if (query.Count() > 0)
            {
                resp.StatusCode = 404;
                resp.StatusMessage = "Success.";
                resp.Results = query.ToList();
                return resp;
            }

            resp.StatusCode = 404;
            resp.StatusMessage = "Ingredient not found.";
            return resp;
        }
    }
}
