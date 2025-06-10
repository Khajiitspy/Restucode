using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Product
{
    public class ProductImageModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public short Priority { get; set; }
    }
}
