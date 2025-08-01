﻿using Core.Models.General;
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
        public Task<PagedResult<ProductItemViewModel>> List(ProductSearchModel filter);

        public Task<ProductDetailsViewModel> Details(long id);
        public Task<ProductVariantEdit> GetVariant(long id);

        public Task<long> CreateProduct(ProductCreateModel model);

        public Task<IEnumerable<IngredientModel>> GetIngredientsAsync();
        public Task<IEnumerable<ProductSizeModel>> GetSizesAsync();

        public Task<long> EditProduct(ProductEditModel model);

        public Task<bool> DeleteProductVariant(long id);

        public Task<IngredientModel> UploadIngredient(CreateIngredientModel model);
    }
}
