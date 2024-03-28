using BL.Models;
using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVC.CookInstructionService
{
    public class CookInstructionService:ICookInstructionService
    {
        private readonly DataContext _context;

        public CookInstructionService(DataContext context)
        {
            _context = context;
        }
        public async Task<Response<CookInstruction>> GetCookInstruction(Guid cookInstructionId)
        {
            Response<CookInstruction> resp = new Response<CookInstruction>();

            if (cookInstructionId == Guid.Empty)
            {
                resp.StatusCode = 404;
                resp.StatusMessage = "Cook Instruction not found.";
                return resp;
            }

            var query = _context.CookInstructions
                .Where(ci => ci.CookInstructionId == cookInstructionId && !ci.IsDeleted);

            if (query.Count() > 0)
            {
                resp.StatusCode = 404;
                resp.StatusMessage = "Success.";
                resp.Results = query.ToList();
                return resp;
            }

            resp.StatusCode = 404;
            resp.StatusMessage = "Cook Instruction not found.";
            return resp;
        }
    }
}
