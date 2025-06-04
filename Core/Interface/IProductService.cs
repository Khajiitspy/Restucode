using Core.Models.General;
using Core.Models.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface
{
    public interface IProductService
    {
        public Task<PagedResult<ProductItemViewModel>> List(string? search, int page = 1, int pageSize = 5);

        public Task<ProductDetailsViewModel> Details(long id);

        public Task<long> CreateProduct(ProductCreateModel model);
    }
}
