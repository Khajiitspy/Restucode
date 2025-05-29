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
                .NotEmpty()
                .WithMessage("Назва є обов'язковою")
                .MaximumLength(250)
                .WithMessage("Назва повинна містити не більше 250 символів")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Name)
                        .MustAsync(async (name, cancellation) =>
                            !await db.Categories.AnyAsync(c => c.Name.ToLower() == name.ToLower().Trim(), cancellation))
                        .WithMessage("Категорія з такою назвою вже існує");
                });

            RuleFor(x => x.Image)
                .NotEmpty()
                .WithMessage("Файл зображення є обов'язковим");
        }
    }
}
