using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Domain;
using Core.Models.Category;

namespace Core.Validators.Category
{
    public class CategoryCreateValidator: AbstractValidator<CategoryAddModel>
    {
        public CategoryCreateValidator(RestucodeDBContext db)
        {
            RuleFor(x => x.Name)
                .MustAsync(async (name, ct) => 
                    await db.Categories.AllAsync(c => c.Name != name, ct)
                ).WithMessage("Category name already exists.");


            RuleFor(x => x.Image)
                .NotEmpty()
                .WithMessage("Файл зображення є обов'язковим");
        }
    }
}
