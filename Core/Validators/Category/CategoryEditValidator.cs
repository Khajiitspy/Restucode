
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Domain;
using Core.Models.Category;
using System.Threading;

public class CategoryEditValidator : AbstractValidator<CategoryEditModel>
{
    public CategoryEditValidator(RestucodeDBContext db)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Назва є обов'язковою")
            .Must(name => !string.IsNullOrEmpty(name)).WithMessage("Назва є обов'язковою")
            .MaximumLength(250).WithMessage("Назва повинна містити не більше 250 символів");

        RuleFor(x => x.Image)
            .Must(file => file == null || file.Length < 2 * 1024 * 1024)
            .WithMessage("Зображення має бути менше 2MB");
    }
}
