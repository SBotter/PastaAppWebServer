using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    public class Response<T>
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; } = string.Empty;
        public List<T> Results { get; set; } = new List<T>();
    }
}
