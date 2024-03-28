using BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVC.IngredientService
{
    public interface IIngredientService
    {
        Task<Response<Ingredient>> GetIngredient(Guid ingredientId);
    }
}
