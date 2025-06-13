using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Product
{
    public class CreateIngredientModel
    {
        public string Name { get; set; } = string.Empty;
        public IFormFile? ImageUrl { get; set; }
    }
}
