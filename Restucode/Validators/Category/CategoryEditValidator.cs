//using FluentValidation;
//using Microsoft.EntityFrameworkCore;
//using Restucode.Data;
//using Restucode.Models.Category;

//public class CategoryEditValidator : AbstractValidator<CategoryEditModel>
//{
//    public CategoryEditValidator(RestucodeDBContext db)
//    {
//        RuleFor(x => x.Name)
//            .NotEmpty().WithMessage("Назва є обов'язковою")
//            .MaximumLength(250).WithMessage("Назва повинна містити не більше 250 символів")
//            .MustAsync(async (name, cancellation) =>
//              !await db.Categories.AnyAsync(c => (c.Name == name && c.Id != ), cancellation))
//            .WithMessage("Категорія з такою назвою вже існує");

//        RuleFor(x => x.Image)
//            .Must(file => file == null || file.Length < 2 * 1024 * 1024)
//            .WithMessage("Зображення має бути менше 2MB");
//    }
//}

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Restucode.Data;
using Restucode.Models.Category;
using System.Threading;

public class CategoryEditValidator : AbstractValidator<CategoryEditModel>
{
    public CategoryEditValidator(RestucodeDBContext db)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Назва є обов'язковою")
            .MaximumLength(250).WithMessage("Назва повинна містити не більше 250 символів")
            .MustAsync(async (model, name, cancellationToken) =>
              !await db.Categories.AnyAsync(x => (x.Name == name && x.Id != model.Id), cancellationToken))
            .WithMessage("Категорія з такою назвою вже існує");

        RuleFor(x => x.Image)
            .Must(file => file == null || file.Length < 2 * 1024 * 1024)
            .WithMessage("Зображення має бути менше 2MB");
    }
}
