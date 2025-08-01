﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;

namespace Domain.Entities
{
    [Table("tblCategories")]
    public class CategoryEntity:BaseEntity<long>
    {
        [StringLength(250)]
        public string Name { get; set; } = string.Empty;
        [StringLength(200)]
        public string Image { get; set; } = string.Empty;
        [StringLength(200)]
        public string Slug { get; set; } = string.Empty;

        public ICollection<ProductVariantEntity>? Products { get; set; } = new List<ProductVariantEntity>();
    }
}
