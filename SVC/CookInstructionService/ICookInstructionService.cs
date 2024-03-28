using BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVC.CookInstructionService
{
    public interface ICookInstructionService
    {
        Task<Response<CookInstruction>> GetCookInstruction(Guid cookInstructionId);
    }
}
