﻿using Core.Models.Category;
using Core.Models.General;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface
{
    public interface ICategoryService
    {
        public Task<CategoryItemViewModel> Create(CategoryAddModel model);

        public Task<CategoryItemViewModel> GetItemById(long Id);

        public Task<PagedResult<CategoryItemViewModel>> ListAsyncPaged(int page, int pageSize, string? search);
        public Task<IEnumerable<CategoryItemViewModel>> ListAsync();

        public Task<CategoryItemViewModel> Edit(long id, CategoryEditModel model);

        public Task<bool> Delete(long Id);
    }
}
