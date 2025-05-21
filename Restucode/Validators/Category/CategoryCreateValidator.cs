using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Restucode.Data;
using Restucode.Models.Category;

namespace Restucode.Validators.Category
{
    public class CategoryCreateValidator: AbstractValidator<CategoryAddModel>
    {
        public CategoryCreateValidator(RestucodeDBContext db)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Назва є обов'язковою")
                .MaximumLength(250)
                .WithMessage("Назва повинна містити не більше 250 символів")
                .MustAsync(async (name, cancellation) =>
                    !await db.Categories.AnyAsync(c => c.Name == name, cancellation))
                .WithMessage("Категорія з такою назвою вже існує");

            RuleFor(x => x.Image)
                .NotEmpty()
                .WithMessage("Файл зображення є обов'язковим");
        }
    }
}
